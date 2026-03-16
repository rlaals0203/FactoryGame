using EPOOutline;
using Factory.Machine;
using UnityEngine;

namespace Code.Factory
{
    public interface IBuildable
    {
        Outlinable Outline { get; set; }
        Vector3Int CellPosition { get; set; }
        MachineSO MachineSO { get; set; }

        void BuildBuilding(Vector3 position, Quaternion rotation);
        void DestroyBuilding();
        void HighlightBuilding();
        void SelectBuilding();
        void DeselectBuilding();
    }
}