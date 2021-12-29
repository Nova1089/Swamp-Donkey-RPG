using RPG.Shops;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.UI.Shops
{
    public class ShopUI : MonoBehaviour
    {
        //configs
        [SerializeField] private TextMeshProUGUI shopName;
        [SerializeField] private Transform listRoot;
        [SerializeField] private RowUI rowPrefab;
        [SerializeField] private TextMeshProUGUI totalField;

        // cached references
        GameObject player;
        Shopper shopper;
        Shop currentShop;

        void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            shopper = player.GetComponent<Shopper>();
        }

        void OnEnable()
        {
            shopper.OnActiveShopChanged += CurrentShopChanged;
        }

        void OnDisable()
        {
            // shopper.OnActiveShopChanged -= ShopChanged;
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
                shopName.text = currentShop.GetShopName();
                currentShop.onChange += RefreshUI;
                gameObject.SetActive(true);
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
        }

        public void ConfirmTransaction()
        {
            currentShop.ConfirmTransaction();
        }

        public void Quit()
        {
            shopper.SetActiveShop(null);
        }
    }
}
