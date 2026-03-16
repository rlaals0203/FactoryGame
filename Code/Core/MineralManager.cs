using System;
using Code.Core.EventSystems;
using Code.Factory;
using Factory.Machine.MiscMachine;
using GondrLib.Dependencies;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;

namespace Works.Factory.Code.Core
{
    public class MineralManager : MonoBehaviour
    {
        [Inject] private PoolManagerMono _poolManager;

        private void Awake()
        {
            GameEventBus.AddListener<PopMineralEvent>(HandlePopPool);
            GameEventBus.AddListener<PushMineralEvent>(HandlePushPool);
            GameEventBus.AddListener<UpgradeMineralEvent>(HandleUpgradeMineral);
        }

        private void OnDestroy()
        {
            GameEventBus.RemoveListener<PopMineralEvent>(HandlePopPool);
            GameEventBus.RemoveListener<PushMineralEvent>(HandlePushPool);
            GameEventBus.RemoveListener<UpgradeMineralEvent>(HandleUpgradeMineral);
        }

        private void HandlePushPool(PushMineralEvent evt)
        {
             _poolManager.Push(evt.poolable);
        }
        
        private void HandleUpgradeMineral(UpgradeMineralEvent evt)
        {
            Vector3 pos = evt.prevMineral.transform.position;
            BaseConveyor conv = GetConveyor(pos);
            if (conv != null)
            {
                Mineral mineral = _poolManager.Pop<IPoolable>(evt.poolItem) as Mineral;
                mineral.transform.position = pos;

                if (conv.GetNextConveyor(out BaseConveyor nextConveyor))
                    mineral.CurrentConveyor = nextConveyor;
                else
                    mineral.PushMineral();
                
                conv.MineralStorage.Add(mineral);
            }

            evt.prevMineral.transform.position = Vector3.zero;
            evt.prevMineral.PushMineral();
        }

        private void HandlePopPool(PopMineralEvent evt)
        {
            BaseConveyor conv = GetConveyor(evt.position);
            
            if (conv != null)
            {
                Mineral mineral = _poolManager.Pop<IPoolable>(evt.poolItem) as Mineral;
                mineral.transform.position = evt.position;
                conv.ConnectMineral(mineral);
            }
        }

        private BaseConveyor GetConveyor(Vector3 position)
        {
            Vector3Int cellPos = BuildManager.Instance.grid.WorldToCell(position);
            BaseConveyor conv = BuildManager.Instance.GetMachineFromCellPos(cellPos) as BaseConveyor;
            return conv;
        }
    }
}