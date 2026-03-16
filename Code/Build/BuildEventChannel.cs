using System.Numerics;
using Code.Core.EventSystems;
using Factory.Machine;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;

namespace Code.Factory
{
    public static class BuildEventChannel
    {
        public static BuildEvent BuildEvent = new BuildEvent();
        public static MachineSelectEvent MachineSelectEvent = new MachineSelectEvent();
        public static ChangeDeployModeEvent ChangeDeployModeEvent = new ChangeDeployModeEvent();
        public static MachineDeselectEvent MachineDeselectEvent = new MachineDeselectEvent();
        public static MachineFixEvent MachineFixEvent = new MachineFixEvent();
        public static MachineRemoveEvent MachineRemoveEvent = new MachineRemoveEvent();
        public static ChangeBuildModeEvent ChangeBuildModeEvent = new ChangeBuildModeEvent();
    }

    public class BuildEvent : GameEvent
    {
        public IBuildable building;
        public Vector3Int cellPos;
        public Quaternion rotation;

        public BuildEvent Initializer(IBuildable newBuilding, Vector3Int newPosition, Quaternion rot)
        {
            building = newBuilding;
            cellPos = newPosition;
            rotation = rot;
            return this;
        }
    }

    public class MachineSelectEvent : GameEvent
    {
        public Machine selectedBuilding;

        public MachineSelectEvent Iniailizer(Machine building)
        {
            selectedBuilding = building;
            return this;
        }
    }
    
    public class MachineDeselectEvent : GameEvent { }

    public class MachineRemoveEvent : GameEvent
    {
        public Machine machine;

        public MachineRemoveEvent Iniailizer(Machine _machine)
        {
            machine = _machine;
            return this;
        }
    }

    public class MachineFixEvent : GameEvent
    {
        public Machine machine;

        public MachineFixEvent Iniailizer(Machine _machine)
        {
            machine = _machine;
            return this;
        }
    }

    public class ChangeDeployModeEvent : GameEvent
    {
        public bool canDeploy = true;

        public ChangeDeployModeEvent Initializer(bool _canDeploy)
        {
            canDeploy = _canDeploy;
            return this;
        }
    }
    
    public class ChangeBuildModeEvent : GameEvent
    {
        public bool canBuild = true;

        public ChangeBuildModeEvent Initializer(bool canBuild)
        {
            this.canBuild = !canBuild;
            return this;
        }
    }
}