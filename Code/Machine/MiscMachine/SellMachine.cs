using Factory.Machine.MiscMachine;
using UnityEngine;
using Works.Inventory.Code;

namespace Factory.Machine.UpgradeMachine
{
    public class SellMachine : BaseConveyor
    {
        [SerializeField] private InventoryDataListSO inventory;
        
        public override void ConveyorAction(Mineral mineral)
        {
            base.ConveyorAction(mineral);
            inventory.AddItem(mineral.MineralSo.itemData);
            mineral.PushMineral();
        }
    }
}