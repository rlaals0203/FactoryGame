using Code.Core.EventSystems;
using Factory.Machine.MiscMachine;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;

namespace Works.Factory.Code.Core
{
    public static class ConveyorEventChannel
    {
        public static PopMineralEvent PopMineralEvent = new();
        public static PushMineralEvent PushMineralEvent = new();
        public static UpgradeMineralEvent UpgradeMineralEvent = new();
    }
    
    public class PopMineralEvent : GameEvent
    {
        public Vector3 position;
        public PoolItemSO poolItem;

        public PopMineralEvent Initializer(Vector3 pos, PoolItemSO pool)
        {
            this.position = pos;
            this.poolItem = pool;
            return this;
        }
    }
    
    public class UpgradeMineralEvent : GameEvent
    {
        public PoolItemSO poolItem;
        public Mineral prevMineral;

        public UpgradeMineralEvent Initializer(PoolItemSO pool, Mineral mineral)
        {
            this.poolItem = pool;
            this.prevMineral = mineral;
            return this;
        }
    }


    public class PushMineralEvent : GameEvent
    {
        public IPoolable poolable;

        public PushMineralEvent Initializer(IPoolable pool)
        {
            this.poolable = pool;
            return this;
        }
    }
}