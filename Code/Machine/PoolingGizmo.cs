using GondrLib.ObjectPool.RunTime;
using UnityEngine;

namespace Factory.Machine
{
    public class PoolingGizmo : MonoBehaviour, IPoolable
    {
        [field:SerializeField] public PoolItemSO PoolItem { get; private set; }
        public GameObject GameObject => gameObject;
        private Pool _myPool;
        private Machine _machine;
        public void SetUpPool(Pool pool)
        {
            _myPool = pool;
        }

        public void ResetItem()
        {
            
        }
    }
}