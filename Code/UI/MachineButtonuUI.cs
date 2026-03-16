using System;
using Code.Core.EventSystems;
using Factory.Machine;
using Players;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Works.Factory.Code.Core;

namespace Works.Factory.Code.UI
{
    public class MachineButtonuUI : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI price;
        
        private Button _button;
        private MachineSO _currentMachine;
        private bool _isStarted = false;

        private readonly MachineUISelectEvent _machineUISelectEvent 
            = MachineUIEventChannel.MachineUISelectEvent;

        private void Awake()
        {
            _button = GetComponentInChildren<Button>();
            _button.onClick.AddListener(HandleUIClick);
        }

        private void HandleUIClick()
        {
            _machineUISelectEvent.Initializer(_currentMachine);
            GameEventBus.RaiseEvent(_machineUISelectEvent);
        }

        public void Refresh(MachineSO machine)
        {
            gameObject.SetActive(machine);
            if (machine == null) return;
            icon.sprite = machine.machineIcon;
            price.text = machine.price.ToString();
            _currentMachine = machine;
        }
    }
}