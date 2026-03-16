using System;
using TMPro;
using UnityEngine;
using Works.Shop.Code;

namespace Works.Factory.Code.UI
{
    public class CurrencyViewer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI currencyText;
        [SerializeField] private CurrencyDataSO currencyData;

        private void Awake()
        {
            currencyData.OnCurrencyChanged += HandleCurrencyChanged;
            currencyText.text = currencyData.Cash.ToString();
        }

        private void HandleCurrencyChanged(int prev, int next)
        {
            currencyText.text = next.ToString();
        }
    }
}