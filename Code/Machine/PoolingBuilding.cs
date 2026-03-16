using Code.Factory;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;

namespace Factory.Machine
{
    public class PoolingBuilding : MonoBehaviour, IPoolable
    {
        [field:SerializeField] public PoolItemSO PoolItem { get; private set; }
        public GameObject GameObject => gameObject;
        private Pool _myPool;
        private IBuildable _buildable;
        public void SetUpPool(Pool pool)
        {
            _myPool = pool;
            _buildable = GetComponent<IBuildable>();
        }

        public void ResetItem()
        {
            
        }

        public IBuildable GetBuildable() => _buildable;
    }
}