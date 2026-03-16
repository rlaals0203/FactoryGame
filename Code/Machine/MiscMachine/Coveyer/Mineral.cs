using System;
using Code.Core.EventSystems;
using Code.Factory;
using DG.Tweening;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;
using Works.Factory.Code.Core;

namespace Factory.Machine.MiscMachine
{
    public class Mineral : MonoBehaviour, IPoolable
    {
        [field: SerializeField] public PoolItemSO PoolItem { get; private set; }
        [field: SerializeField] public MineralSO MineralSo { get; private set; }
        public GameObject GameObject => gameObject;
        public float ChangedTime { get; set; }
        public bool IsConnecting { get; set; } = false;
        public BaseConveyor CurrentConveyor { get; set; } = null;

        private readonly PushMineralEvent _pushEvt = ConveyorEventChannel.PushMineralEvent;
        private readonly PopMineralEvent _popEvt = ConveyorEventChannel.PopMineralEvent;
        private readonly UpgradeMineralEvent _upgradeEvt = ConveyorEventChannel.UpgradeMineralEvent;


        private void Update()
        {
            if (Time.time - ChangedTime > 5f)
            {
                PushMineral();
            }
        }

        public void UpgradeResource(Mineral mineral)
        {
            if (!mineral.MineralSo.proessable) return;
            PoolItemSO pool = mineral.MineralSo.processedMineral.mineralPool;
            GameEventBus.RaiseEvent(_upgradeEvt.Initializer(pool, mineral));
        }
        
        public void PopResource(Vector3 pos, PoolItemSO pool)
            => GameEventBus.RaiseEvent(_popEvt.Initializer(pos, pool));
        
        public void PushMineral()
            => GameEventBus.RaiseEvent(_pushEvt.Initializer(this));

        public void SetUpPool(Pool pool) { }

        public void ResetItem()
        {
            ChangedTime = Time.time;
            transform.position = Vector3.zero;
            CurrentConveyor = null;
            IsConnecting = false;
            transform.DOKill();   
        }
    }
}