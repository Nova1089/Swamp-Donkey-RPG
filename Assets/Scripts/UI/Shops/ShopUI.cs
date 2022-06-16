using RPG.Shops;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Shops
{
    public class ShopUI : MonoBehaviour
    {
        //configs
        [SerializeField] private TextMeshProUGUI shopName;
        [SerializeField] private Transform listRoot;
        [SerializeField] private RowUI rowPrefab;
        [SerializeField] private TextMeshProUGUI totalField;
        [SerializeField] private Button buySellButton;
        [SerializeField] private Button switchBuySellButton;

        // cached references
        TextMeshProUGUI switchButtonText;
        TextMeshProUGUI buySellButtonText;

        // state
        GameObject player;
        Shopper shopper;
        Shop currentShop;
        Color originalTotalTextColor;

        void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            shopper = player.GetComponent<Shopper>();
            originalTotalTextColor = totalField.color;
            switchButtonText = switchBuySellButton.GetComponentInChildren<TextMeshProUGUI>();
            buySellButtonText = buySellButton.GetComponentInChildren<TextMeshProUGUI>();
        }

        void OnEnable()
        {
            shopper.OnActiveShopChanged += CurrentShopChanged;
            buySellButton.onClick.AddListener(ConfirmTransaction);
            switchBuySellButton.onClick.AddListener(SwitchBuySellMode);
        }

        void OnDisable()
        {
            buySellButton.onClick.RemoveAllListeners();
            switchBuySellButton.onClick.RemoveAllListeners();
        }

        // Start is called before the first frame update
        void Start()
        {
            if (shopper == null) return;
            CurrentShopChanged();
        }

        private void CurrentShopChanged()
        {
            if (currentShop != null)
            {
                currentShop.onChange -= RefreshUI;
            }
            
            currentShop = shopper.GetActiveShop();

            if (currentShop != null)
            {
                foreach (FilterButtonUI button in GetComponentsInChildren<FilterButtonUI>())
                {
                    button.SetShop(currentShop);
                }

                shopName.text = currentShop.GetShopName();                
                gameObject.SetActive(true);
                currentShop.onChange += RefreshUI;
            }
            else
            {
                gameObject.SetActive(false);
            }

            RefreshUI();
        }

        private void RefreshUI()
        {
            foreach (Transform child in listRoot)
            {
                Destroy(child.gameObject);
            }

            if (currentShop == null) return;
            foreach (ShopItem item in currentShop.GetFilteredItems())
            {
                RowUI row = Instantiate(rowPrefab, listRoot);
                row.Setup(currentShop, item, item.GetQuantityInTransaction());
            }

            totalField.text = $"Total: ${currentShop.TransactionTotal():N2}";
            totalField.color = currentShop.HasSufficientFunds() ? originalTotalTextColor : Color.red;

            buySellButton.interactable = currentShop.CanTransact();

            if (currentShop.IsBuyingMode())
            {
                switchButtonText.text = "Switch To Selling";
                buySellButtonText.text = "Buy";
            }
            else
            {
                switchButtonText.text = "Switch To Buying";
                buySellButtonText.text = "Sell";
            }

            foreach (FilterButtonUI button in GetComponentsInChildren<FilterButtonUI>())
            {
                button.RefreshUI();
            }
        }

        public void ConfirmTransaction()
        {
            currentShop.ConfirmTransaction();
        }

        public void SwitchBuySellMode()
        {
            currentShop.SwitchBuySellMode(!currentShop.IsBuyingMode());
        }

        // called by Shop UI Quit Button
        public void Quit()
        {
            shopper.CancelShopping();
        }
    }
}
