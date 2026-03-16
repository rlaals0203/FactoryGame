using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Works.Shop.Code;

namespace Works.Factory.Code.UI
{
    public class LandButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private CurrencyDataSO currencyData;

        public event Action<LandButton> OnSelected;
        private List<Action<LandButton>> _evtTable = new();

        public int Index {get; private set;}
        public int Price {get; private set;}

        private Button _landButton;
        private Image _landImage;
        private bool _isPurchased;

        private const int Tier1 = 1000;
        private const int Tier2 = 2500;
        private const int Tier3 = 5000;
        
        private readonly Color _activeColor = new Color(1f, 1f, 1f);
        private readonly Color _inActiveColor = new Color(0.5f, 0.5f, 0.5f);
        
        public void Awake()
        {
            _landButton = GetComponent<Button>();
            _landImage = GetComponent<Image>();
            
            Index = transform.GetSiblingIndex();
            _landImage.color = _inActiveColor;

            if(Index == 0 || Index == 4 || Index == 5 || (Index >= 9 && Index <= 14))
                Price = Tier1;
            else if (Index >= 15 && Index <= 19)
                Price = Tier2;
            else if (Index >= 20 && Index <= 24)
                Price = Tier3;
            else
            {
                _landImage.color = _activeColor;
                _landButton.interactable = false;
                _isPurchased = true;
            }
            
            if (PlayerPrefs.GetInt($"Land{Index}", 0) == 1)
            {
                SetPurchased();
                return;
            }

            
            _landButton.onClick.AddListener(HandlePurchase);
        }

        private void OnDestroy()
        {
            foreach (Action<LandButton> evt in _evtTable)
            {
                OnSelected -= evt;
            }

            _landButton.onClick.RemoveAllListeners();
        }
        
        private void HandlePurchase()
        {
            if (!currencyData.HasCash(Price)) return;
            
            currencyData.RemoveCash(Price);
            SetPurchased();
            
            PlayerPrefs.SetInt($"Land{Index}", 1);
            PlayerPrefs.Save();
        }

        private void SetPurchased()
        {
            _landImage.color = _activeColor;
            _landButton.interactable = false;
            _isPurchased = true;
        }

        public void SubscribeLandSelected(Action<LandButton> callback)
        {
            OnSelected += callback;
            _evtTable.Add(callback);
        }
        
        public void SubscribeLandPurchase(UnityAction callback)
            => _landButton.onClick.AddListener(callback);

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(!_isPurchased)
                OnSelected?.Invoke(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnSelected?.Invoke(null);
        }
    }
}