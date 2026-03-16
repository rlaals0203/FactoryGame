using GondrLib.ObjectPool.RunTime;
using UnityEngine;

public enum MachineType
{
    Mining,
    Conveyor,
    Upgrader,
    Everything
}

namespace Factory.Machine
{
    [CreateAssetMenu(fileName = "MachineSO", menuName = "SO/Factory/Machine", order = 0)]
    public class MachineSO : ScriptableObject
    {
        [Header("Info Setting")]
        public Sprite machineIcon;
        public string machineName;
        [TextArea] public string description;
        public PoolItemSO machinePool;
        public PoolItemSO machineGizmo;
        
        [Header("Shop Setting")]
        public int price;
        public MachineType machineType;
    }
}