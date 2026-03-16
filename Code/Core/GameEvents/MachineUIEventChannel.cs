using Code.Core.EventSystems;
using Factory.Machine;

namespace Works.Factory.Code.Core
{
    public static class MachineUIEventChannel
    {
        public static MachineTypeUIEvent MachineTypeUIEvent = new MachineTypeUIEvent();
        public static MachineUISelectEvent MachineUISelectEvent = new MachineUISelectEvent();
    }

    public class MachineTypeUIEvent : GameEvent
    {
        public MachineType newType;

        public MachineTypeUIEvent Initializer(MachineType type)
        {
            newType = type;
            return this;
        }
    }

    public class MachineUISelectEvent : GameEvent
    {
        public MachineSO machine;

        public MachineUISelectEvent Initializer(MachineSO machine)
        {
            this.machine = machine;
            return this;
        }
    }
}