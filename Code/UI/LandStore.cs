using Code.Core.EventSystems;
using GondrLib.Dependencies;
using Mining.Events;
using Players;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Works.Shop.Code;

namespace Works.Factory.Code.UI
{
    public class LandStore : MonoBehaviour
    {
        [SerializeField] private LandButton[] landButtons;
        [SerializeField] private GameObject[] landObjects;
        [SerializeField] private TextMeshProUGUI landText;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private Button purchaseButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private GameObject background;
        [SerializeField] private CurrencyDataSO currencyData;

        private LandButton _currentLand;
        [Inject] private Player _player;
        private void Start()
        {
            background.SetActive(true);
            foreach (var land in landButtons)
            {
                land.SubscribeLandSelected(HandleLandChange);
                land.SubscribeLandPurchase(HandleLandPurchase);
            }

            for (int i = 0; i < landObjects.Length; i++)
            {
                if (PlayerPrefs.GetInt($"Land{i}", 0) == 1)
                {
                    landObjects[i].SetActive(true);
                }
            }
            
            closeButton.onClick.AddListener(HandleCloseStore);
            background.SetActive(false);
        }

        private void OnDestroy()
        {
            closeButton.onClick.RemoveAllListeners();
        }

        private void HandleLandPurchase()
        {
            if (currencyData.HasCash(_currentLand.Price))
                landObjects[_currentLand.Index].SetActive(true);
        }

        private void HandleLandChange(LandButton land)
        {
            _currentLand = land;
            
            if (_currentLand == null)
            {
                TryClearLand();
            }
            else
            {
                landText.text = $"부지 {_currentLand.Index + 1}번";
                priceText.text = $"{_currentLand.Price}원";
                purchaseButton.gameObject.SetActive(true);
            }
        }

        private async void TryClearLand()
        {
            await Awaitable.WaitForSecondsAsync(1f);
            if (_currentLand != null) return;
            
            landText.text = string.Empty;
            priceText.text = string.Empty;
            purchaseButton.gameObject.SetActive(false);
        }

        private void HandleCloseStore()
        {
            GameEventBus.RaiseEvent(CursorEvents.OnOffCursorEvent.SetEvent(false, gameObject));
            _player.OnOffPlayer(true);
            background.SetActive(false);
        }
    }
}
