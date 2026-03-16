using GondrLib.ObjectPool.RunTime;
using UnityEngine;
using Works.Items.Code;

namespace Factory.Machine.MiscMachine
{
    [CreateAssetMenu(fileName = "MineralSO", menuName = "SO/Mineral", order = 0)]
    public class MineralSO : ScriptableObject
    {
        public ItemDataSO itemData;
        public PoolItemSO mineralPool;
        public MineralSO processedMineral;
        public bool proessable;
    }
}