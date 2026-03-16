using System;
using UnityEngine;

namespace Factory.Machine.MiscMachine
{
    [CreateAssetMenu(fileName = "ConveyorSO", menuName = "SO/Conveyor", order = 0)]
    public class ConveyorSO : MachineSO
    {
        public float conveyorSpeed = 1f;
    }
}