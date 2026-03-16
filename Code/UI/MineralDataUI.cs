using Factory.Machine.MiscMachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Works.Items.Code;

namespace Works.Factory.Code.UI
{
    public class MineralDataUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private TextMeshProUGUI mineralText;
        [SerializeField] private Image icon;

        private int amount = 0;

        public void SetUpUI(ItemDataSO itemData, int amount)
        {
            icon.sprite = itemData.itemIcon;
            mineralText.text = itemData.itemName;
            amountText.text = amount.ToString();
        }
        
        public void RefreshUI()
        {
            amount++;
            amountText.text = amount.ToString();
        }
    }
}