using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Works.Factory.Code.UI
{
    public enum ToggleDirection
    {
        Horizontal,
        Vertical
    }
    
    public class UIToggler : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTrm;
        [SerializeField] private float distance;
        [SerializeField] private float duration;
        [SerializeField] private Ease easeType;
        [SerializeField] private ToggleDirection mode;
        
        private bool _isToggled = false;
        private Button _button;
        private Vector3 _startPos;
        private Vector3 _endPos;

        private void Awake()
        {
            _button = GetComponent<Button>();
            
            _startPos = rectTrm.transform.position;
            _endPos = mode == ToggleDirection.Horizontal
                ? new Vector2(_startPos.x + distance, _startPos.y)
                : new Vector2(_startPos.x, _startPos.y + distance);
            
            _button.onClick.AddListener(ToggleUI);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(ToggleUI);
        }

        private void ToggleUI()
        {
            _isToggled = !_isToggled;
            
            if (_isToggled)
                rectTrm.DOMove(_endPos, duration).SetEase(easeType);
            else
                rectTrm.DOMove(_startPos, duration).SetEase(easeType);
        }
    }
}