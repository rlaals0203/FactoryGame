using Factory.Machine.MiscMachine;
using UnityEngine;

namespace Factory.Machine.UpgradeMachine
{
    public class UpgradeMachine : BaseConveyor
    {
        private UpgradeMachineSO _upgradeMachine;

        protected override void InitializeMachine()
        {
            base.InitializeMachine();
            _upgradeMachine = machineSO as UpgradeMachineSO;
        }

        public override void ConveyorAction(Mineral mineral)
        {
            base.ConveyorAction(mineral);

            float rand = Random.value;
            if (rand <= _upgradeMachine.rarity)
            {
                RemoveMineral(mineral);
                mineral.UpgradeResource(mineral);
            }
        }
    }
}