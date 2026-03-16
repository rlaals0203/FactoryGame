using System;
using Code.Factory;
using EPOOutline;
using UnityEngine;

namespace Factory.Machine
{
    [RequireComponent(typeof(Outlinable))]
    public abstract class Machine : MonoBehaviour, IBuildable
    {
        [field: SerializeField] public MachineSO machineSO { get; private set; }
        public Outlinable Outline { get; set; }
        public Vector3Int CellPosition { get; set; }
        public MachineSO MachineSO { get; set; }

        protected virtual void InitializeMachine() { }

        private void OnEnable()
        {
            if (Outline == null)
                Outline = GetComponent<Outlinable>();
            
            Outline.enabled = false;
        }

        public virtual void BuildBuilding(Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;
            InitializeMachine();
        }
        
        public virtual void DestroyBuilding() { }
        
        public virtual void HighlightBuilding()
        {
            Outline.enabled = true;
            Outline.OutlineParameters.Color = Color.white;
        }

        public virtual void SelectBuilding() { }

        public virtual void DeselectBuilding() { }
    }
}
