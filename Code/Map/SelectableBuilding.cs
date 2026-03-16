using EPOOutline;
using UnityEngine;

namespace Works.Factory.Code.Map
{
    [RequireComponent(typeof(Outlinable))]
    public class SelectableBuilding : MonoBehaviour
    {
        [SerializeField] private Outlinable OutLine { get; set; }

        public void HighlightBuilding()
        {
            OutLine.enabled = true;
        }

        public void SelectBuilding()
        {
            
        }

        public void DeSelectBuilding()
        {
            
        }
    }
}