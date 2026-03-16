using System;
using System.Collections.Generic;
using System.Linq;
using Code.Core;
using Code.Core.EventSystems;
using Core.GameSystem;
using Factory.Machine;
using Factory.Machine.MiscMachine;
using GondrLib.Dependencies;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Factory
{
    [Serializable]
    public class BuildData {
        public MachineSO machineSO;
        public Vector3Int cellPos;
        public Quaternion rotation;
    }
    
    public class MachineData
    {
        public Machine machine;
        public PoolingBuilding pool;
    }
    
    public class BuildManager : MonoSingleton<BuildManager>
    {
        [Inject] private PoolManagerMono _poolManager;
        public event Action<MachineData> OnBuild;
        public Grid grid;
        public Dictionary<IBuildable, Vector3Int> buildingDict = new();
        public Dictionary<Vector3Int, IBuildable> positionDict = new();
        public static List<BuildData> saveDataList = new();
        
        private readonly int[] _dx = { 1, -1, 0, 0 };
        private readonly int[] _dz = { 0, 0, 1, -1 };

        private Stack<IBuildable> _nearBuilding = new(8);
        private static BuildManager _instance = null;

        private void Awake()
        {
            GameEventBus.AddListener<BuildEvent>(HandleDeployEvent);

            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
        }
        
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        
        private void OnDestroy()
        {
            GameEventBus.RemoveListener<BuildEvent>(HandleDeployEvent);
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            grid = FindObjectOfType<Grid>();
            LoadMachines();
        }

        private void HandleDeployEvent(BuildEvent channel)
        {
            buildingDict.TryAdd(channel.building, channel.cellPos);
            positionDict.TryAdd(channel.cellPos, channel.building);
            RefreshConveyor(channel.building);
            
            saveDataList.RemoveAll(d => d.cellPos == channel.cellPos);
            BuildData data = new BuildData
            {
                machineSO = channel.building.MachineSO,
                cellPos = channel.cellPos,
                rotation = channel.rotation
            };
            saveDataList.Add(data);
        }

        /*public void ChangeBuilding(IBuildable building, Vector3 pos)
        {
            Vector3Int position = buildingDict[building];
            buildingDict.Remove(building);
            positionDict.Remove(position);

            Vector3Int cell = grid.WorldToCell(pos);
            buildingDict.Add(building, cell);
            positionDict.Add(cell, building);
        }*/
        
        public void LoadMachines()
        {
            if (saveDataList == null) return;
            
            buildingDict.Clear();
            positionDict.Clear();

            foreach (var data in saveDataList)
            {
                PoolingBuilding pool = _poolManager.Pop<PoolingBuilding>(data.machineSO.machinePool);
                pool.transform.position = grid.CellToWorld(data.cellPos) + grid.cellSize / 2f;
                pool.transform.rotation = data.rotation;
                
                MachineData machineData = new MachineData();
                machineData.machine = pool.GetComponent<Machine>();
                machineData.pool = pool;
                OnBuild?.Invoke(machineData);
                
                if (pool.TryGetComponent(out IBuildable buildable))
                {
                    buildingDict[buildable] = data.cellPos;
                    positionDict[data.cellPos] = buildable;
                }
                
                if (buildable is BaseConveyor conveyor)
                {
                    conveyor.CheckDirection();
                }
            }

            foreach (var building in buildingDict.Keys)
            {
                RefreshConveyor(building);
            }
        }

        public void RemoveBuilding(IBuildable building, Vector3 positon)
        {
            Vector3Int cellPos = grid.WorldToCell(positon);
            buildingDict.Remove(building);
            positionDict.Remove(cellPos);
            RefreshConveyor(building);
        }

        public IBuildable[] GetNearMachine(GameObject targt)
        {
            Vector3Int cellPos = grid.WorldToCell(targt.transform.position);
            _nearBuilding.Clear();

            for (int i = 0; i < 4; i++)
            {
                Vector3Int newPos = cellPos + new Vector3Int(_dx[i], 0,  _dz[i]);

                if (positionDict.TryGetValue(newPos, out IBuildable buildable))
                {
                    _nearBuilding.Push(buildable);
                }
            }

            return _nearBuilding.ToArray();
        }

        public IBuildable GetMachineFromCellPos(Vector3Int cellPos)
        {
            if (positionDict.TryGetValue(cellPos, out IBuildable buildable))
            {
                return buildable;
            }
            
            return null;
        }

        public bool CheckMachineOnCell(Vector3Int cellPos)
        {
            return positionDict.ContainsKey(cellPos);
        }

        private void RefreshConveyor(IBuildable buildable)
        {
            if (buildable is BaseConveyor conveyor)
            {
                conveyor.TryConnect();

                foreach (var next in GetNearMachine(conveyor.gameObject).OfType<BaseConveyor>())
                {
                    next.TryConnect();
                }
            }
        }
    }
}