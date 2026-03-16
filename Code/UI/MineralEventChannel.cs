using Code.Core.EventSystems;
using Works.Inventory.Code;
using Works.Items.Code;

namespace Works.Factory.Code.UI
{
    public static class MineralEventChannel
    {
        public static MineralAddEvent MineralAddEvent = new MineralAddEvent();
    }

    public class MineralAddEvent : GameEvent
    {
        public ItemDataSO data;

        public MineralAddEvent Initializer(ItemDataSO data)
        {
            this.data = data;
            return this;
        }
    }
}