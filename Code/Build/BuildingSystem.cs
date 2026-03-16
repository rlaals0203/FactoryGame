using System;
using System.Collections.Generic;
using System.Linq;
using Code.Core.EventSystems;
using Core.GameSystem;
using Factory.Machine;
using GondrLib.Dependencies;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Works.Factory.Code.Core;
using Works.Shop.Code;

namespace Code.Factory
{
    public class BuildingSystem : MonoBehaviour
    {
        [SerializeField] public Grid grid;
        [SerializeField] private InputSO playerInput; 
        [SerializeField] private CurrencyDataSO currencyData;
        [Inject] private PoolManagerMono _poolManager;
        
        private Dictionary<Machine, PoolingBuilding> _machineDict = new();
        private MachineSO _curMachine;
        private PoolingBuilding _fixObject;

        private int _rotate;
        private bool _canDeploy;
        private bool _isFixMode = false;

        private readonly BuildEvent _buildEvent = BuildEventChannel.BuildEvent;
        private readonly ChangeDeployModeEvent changeModeEvent = BuildEventChannel.ChangeDeployModeEvent;

        public event Action<Vector3> OnRotateEvent;

        
        public bool CanDeploy
        {
            get => _canDeploy;
            set {
                _canDeploy = value;
                GameEventBus.RaiseEvent(changeModeEvent.Initializer(value));
            }
        }
        
        private void Awake()
        {
            InitializeComponents();
            playerInput.OnClickEvent += HandleClick;
            playerInput.OnRotateEvent += HandleRotateChange;
            playerInput.OnRightClickEvent += HandleCancel;
            BuildManager.Instance.OnBuild += HandleBuild;
            
            GameEventBus.AddListener<MachineUISelectEvent>(HandleMachineSelect);
            GameEventBus.AddListener<MachineRemoveEvent>(HandleRemoveEvent);
            GameEventBus.AddListener<MachineFixEvent>(HandlemachineFix);
        }

        private void OnDestroy()
        {
            playerInput.OnClickEvent -= HandleClick;
            playerInput.OnRotateEvent -= HandleRotateChange;
            playerInput.OnRightClickEvent -= HandleCancel;
            BuildManager.Instance.OnBuild -= HandleBuild;

            GameEventBus.RemoveListener<MachineUISelectEvent>(HandleMachineSelect);
            GameEventBus.RemoveListener<MachineRemoveEvent>(HandleRemoveEvent);
            GameEventBus.RemoveListener<MachineFixEvent>(HandlemachineFix);
        }
        
        private void HandleBuild(MachineData data)
        {
            _machineDict.Add(data.machine, data.pool);
        }
        
        private void HandleRemoveEvent(MachineRemoveEvent evt)
        {
            RemoveMachine(evt.machine);
        }
        
        private void HandlemachineFix(MachineFixEvent evt)
        {
            CanDeploy = true;
            _isFixMode = true;
            _curMachine = evt.machine.machineSO;
            _fixObject = evt.machine.GetComponent<PoolingBuilding>();
        }
        
        private void HandleMachineSelect(MachineUISelectEvent evt)
        {
            CanDeploy = true;
            _curMachine = evt.machine;
        }
        
        private void HandleCancel() => CanDeploy = false;

        private void HandleRotateChange(int dir)
            => OnRotateEvent?.Invoke(new Vector3(0, _rotate += dir * 90, 0));

        private void HandleClick(bool isClick)
        {
            if(isClick && CanDeploy)
                DeployMachine();
        }

        private void RemoveMachine(Machine machine)
        {
            var target = _machineDict[machine];
            _poolManager.Push(target);
            _machineDict.Remove(machine);
            BuildManager.Instance.RemoveBuilding(machine,target.transform.position);
        }

        private void DeployMachine()
        {
            if (!currencyData.HasCash(_curMachine.price) && !_isFixMode) return;
            
            Vector3Int cellPosition = GetCellPoint();
            if (BuildManager.Instance.CheckMachineOnCell(cellPosition)|| !_canDeploy) return;
            
            Vector3 position = grid.GetCellCenterWorld(cellPosition);
            Quaternion rotation = Quaternion.Euler(0, _rotate, 0);
            var buildingPool = _poolManager.Pop<PoolingBuilding>(_curMachine.machinePool);
            IBuildable building = buildingPool.GetBuildable();
            building.MachineSO = _curMachine;

            if (building is Machine machine)
            {
                _machineDict.TryAdd(machine, buildingPool);
            }

            if (_isFixMode)
            {
                Machine fixMachine = _fixObject.GetComponent<Machine>();
                BuildManager.Instance.RemoveBuilding(fixMachine, _fixObject.transform.position);
                _poolManager.Push(_fixObject);
                
                CanDeploy = false;
                _isFixMode = false;
                _fixObject = null;
            }
            else
            {
                currencyData.RemoveCash(_curMachine.price);
            }
            
            building.BuildBuilding(position, rotation);
            GameEventBus.RaiseEvent(_buildEvent.Initializer(building, cellPosition, rotation));
        }

        public Vector3Int GetCellPoint()
        {
            Vector3 worldPosition = playerInput.GetWorldPosition();
            Vector3Int cellPoint = grid.WorldToCell(worldPosition);
            return cellPoint;
        }

        public Vector3 GetCellCenterWorld(Vector3Int position)
            => grid.GetCellCenterWorld(position);
        
        private void InitializeComponents()
        {
            GetComponentsInChildren<IBuildComponent>().ToList()
                .ForEach(component => component.Initialize(this));
        }
    }
}
