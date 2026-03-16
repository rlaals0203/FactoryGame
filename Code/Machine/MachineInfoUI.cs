using System;
using Code.Core.EventSystems;
using Code.Factory;
using Players;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Works.Factory.Code.Core;

namespace Factory.Machine
{
    public class MachineInfoUI : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private RectTransform uiRect;
        [SerializeField] private Transform background;
        [SerializeField] private Button fixButton;
        [SerializeField] private Button removeButton;

        private Machine _currentMachine;
        private MachineFixEvent _machineFixEvent = BuildEventChannel.MachineFixEvent;
        private MachineRemoveEvent _machineRemoveEvent = BuildEventChannel.MachineRemoveEvent;
        private MachineUISelectEvent _machineSelectEvent = MachineUIEventChannel.MachineUISelectEvent;

        private void Awake()
        {
            GameEventBus.AddListener<MachineSelectEvent>(HandleMachineSelect);
            GameEventBus.AddListener<MachineDeselectEvent>(HandleMachineDeselect);
            GameEventBus.AddListener<ChangeBuildModeEvent>(HandleChangeBuildMode);

            
            fixButton.onClick.AddListener(HandleFixClick);
            removeButton.onClick.AddListener(HandleRemoveClick);
        }

        private void OnDestroy()
        {
            GameEventBus.RemoveListener<MachineSelectEvent>(HandleMachineSelect);
            GameEventBus.RemoveListener<MachineDeselectEvent>(HandleMachineDeselect);
            GameEventBus.RemoveListener<ChangeBuildModeEvent>(HandleChangeBuildMode);

            fixButton.onClick.RemoveListener(HandleFixClick);
            removeButton.onClick.RemoveListener(HandleRemoveClick);
        }

        private void InitializeUI(Machine machine)
        {
            _currentMachine = machine;
            MachineSO machineSO = machine.machineSO;
            
            uiRect.gameObject.SetActive(true);
            title.text = machineSO.machineName;
            description.text = machineSO.description;
            icon.sprite = machineSO.machineIcon;
        }
        
        private void HandleChangeBuildMode(ChangeBuildModeEvent evt)
        {
            background.gameObject.SetActive(evt.canBuild);
        }

        private void HandleMachineDeselect(MachineDeselectEvent evt)
        {
            uiRect.gameObject.SetActive(false);
        }

        private void HandleMachineSelect(MachineSelectEvent evt)
            => InitializeUI(evt.selectedBuilding);
        
        private void HandleRemoveClick()
        {
            GameEventBus.RaiseEvent(_machineRemoveEvent.Iniailizer(_currentMachine));
        }

        private void HandleFixClick()
        {
            GameEventBus.RaiseEvent(_machineFixEvent.Iniailizer(_currentMachine));
            GameEventBus.RaiseEvent(_machineSelectEvent.Initializer(_currentMachine.machineSO));
        }
    }
}