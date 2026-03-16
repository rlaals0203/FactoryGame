using System;
using System.Collections.Generic;
using Code.Core.EventSystems;
using Code.Factory;
using Factory.Machine;
using Players;
using UnityEngine;
using Works.Factory.Code.Core;
using Works.Factory.Code.UI;
using Object = UnityEngine.Object;

public class MachineSelectHolderUI : MonoBehaviour
{
    [SerializeField] private MachineSO[] machines;

    private Dictionary<MachineType, List<MachineSO>> _machineDict = new();
    private MachineType _currentType = MachineType.Everything;
    [SerializeField] private MachineButtonuUI machineButton;
    [SerializeField] private Transform contentTrm;
    [SerializeField] private Transform background;
    private List<MachineButtonuUI> _machineList = new();

    private void Awake()
    {
        _machineDict.Add(MachineType.Everything, new List<MachineSO>());
        _machineDict.Add(MachineType.Mining, new List<MachineSO>());
        _machineDict.Add(MachineType.Conveyor, new List<MachineSO>());
        _machineDict.Add(MachineType.Upgrader, new List<MachineSO>());
        
        foreach (var machine in machines)
        {
            _machineDict[machine.machineType].Add(machine);
            _machineList.Add(Instantiate(machineButton, contentTrm));
        }

        foreach (var machineList in _machineDict)
        {
            machineList.Value.Sort((machine1, machine2) 
                => machine1.price.CompareTo(machine2.price));
        }
        
        InitializeMachineUI(_currentType);
        GameEventBus.AddListener<MachineTypeUIEvent>(HandleTypeSelect);
        GameEventBus.AddListener<ChangeBuildModeEvent>(HandleChangeBuildMode);
    }

    private void OnDestroy()
    {
        GameEventBus.RemoveListener<MachineTypeUIEvent>(HandleTypeSelect);
        GameEventBus.RemoveListener<ChangeBuildModeEvent>(HandleChangeBuildMode);
    }
    

    private void HandleChangeBuildMode(ChangeBuildModeEvent evt)
    {
        background.gameObject.SetActive(evt.canBuild);
    }

    private void HandleTypeSelect(MachineTypeUIEvent evt)
    {
        if (_currentType == evt.newType) return;
        _currentType = evt.newType;
        InitializeMachineUI(_currentType);
    }

    private void InitializeMachineUI(MachineType type)
    {
        for (int i = 0; i < machines.Length; i++)
        {
            _machineList[i].Refresh(null);
        }

        if (type == MachineType.Everything)
        {
            int idx = 0;
            foreach (List<MachineSO> machineList in _machineDict.Values)
            {
                foreach (MachineSO machine in machineList)
                {
                    _machineList[idx].Refresh(machine);
                    idx++;
                }
            }

            return;
        }
        
        for (int i = 0; i < _machineDict[type].Count; i++)
        {
            _machineList[i].Refresh(_machineDict[type][i]);
        }
    }
}
