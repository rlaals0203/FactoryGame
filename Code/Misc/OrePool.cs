using GondrLib.ObjectPool.RunTime;
using UnityEngine;

namespace Works.Factory.Code.Misc
{
    public class OrePool : MonoBehaviour, IPoolable
    {
        [field:SerializeField] public PoolItemSO PoolItem { get; private set; }
        public GameObject GameObject => gameObject;
        private Pool _pool;
        
        public void SetUpPool(Pool pool)
        {
            _pool = pool;
        }

        public void ResetItem() { }

        public void PushOre()
        {
            _pool.Push();
        }
    }
}