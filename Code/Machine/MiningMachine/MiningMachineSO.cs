using GondrLib.ObjectPool.RunTime;
using UnityEngine;

namespace Factory.Machine
{
    [CreateAssetMenu(fileName = "MiningMachineSO", menuName = "SO/Factory/MiningMachine", order = 0)]
    public class MiningMachineSO : MachineSO
    {
        [Header("Mining Machine Setting")]
        public PoolItemSO orePool;
        public float coolTime;
    }
}