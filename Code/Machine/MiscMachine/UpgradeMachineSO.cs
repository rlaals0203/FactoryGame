using Factory.Machine.MiscMachine;
using UnityEngine;

namespace Factory.Machine.UpgradeMachine
{
    [CreateAssetMenu(fileName = "UpgradeMachine", menuName = "SO/Machine/UpgradeMachine", order = 0)]
    public class UpgradeMachineSO : ConveyorSO
    {
        public float rarity;
    }
}