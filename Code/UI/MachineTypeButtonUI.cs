using Code.Core.EventSystems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Works.Factory.Code.Core;

namespace Works.Factory.Code.UI
{
    public class MachineTypeButtonUI : MonoBehaviour
    {
        private MachineType _machineType;
        private Button _button;
        private TextMeshProUGUI _text;
        private readonly MachineTypeUIEvent _machineTypeUIEvent 
            = MachineUIEventChannel.MachineTypeUIEvent;

        private string[] _machineTypeName =
        {
            "마이닝",
            "컨베이어",
            "가공기",
            "전체보기"
        };
        
        private void Awake()
        {
            int index = transform.GetSiblingIndex();
            _machineType = (MachineType)index;
            _button = GetComponent<Button>();
            _button.onClick.AddListener(HandleButtonClick);

            _text = GetComponentInChildren<TextMeshProUGUI>();
            _text.text = _machineTypeName[index];
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(HandleButtonClick);
        }

        private void HandleButtonClick()
        {
            _machineTypeUIEvent.Initializer(_machineType);
            GameEventBus.RaiseEvent(_machineTypeUIEvent);
        }
    }
}