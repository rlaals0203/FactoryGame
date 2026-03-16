using System;
using System.Reflection.Emit;
using Code.Core.EventSystems;
using Code.Effects;
using EPOOutline;
using GondrLib.Dependencies;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;
using Works.Factory.Code.Core;
using Works.Factory.Code.Misc;

namespace Factory.Machine
{
    public class MiningMachine : Machine
    {
        [SerializeField] private Transform orePos;

        private MiningMachineSO _miningMachineSO;
        private float _lastTime;

        private readonly PopMineralEvent _popMineralEvent = ConveyorEventChannel.PopMineralEvent;

        protected override void InitializeMachine()
        {
            base.InitializeMachine();
            _miningMachineSO = machineSO as MiningMachineSO;
            _lastTime = Time.time + _miningMachineSO.coolTime - 5;
        }

        private void Update()
        {
            if (_miningMachineSO == null)
            {
                _miningMachineSO = machineSO as MiningMachineSO;
            }
            
            if (Time.time - _lastTime > _miningMachineSO.coolTime)
            {
                SpawnOre();
                _lastTime = Time.time;
            }
        }

        protected virtual void SpawnOre()
        {
            Vector3 pos = orePos.position;
            GameEventBus.RaiseEvent(_popMineralEvent.Initializer(pos, _miningMachineSO.orePool));
        }
    }
}