using RPG.Shops;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Shops
{
    public class RowUI : MonoBehaviour
    {
        // configs
        [SerializeField] private TextMeshProUGUI nameField;
        [SerializeField] private TextMeshProUGUI availabilityField;
        [SerializeField] private TextMeshProUGUI priceField;
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI quantityField;

        // cached references
        Shop shop;
        ShopItem item;

        public void Setup(Shop shop, ShopItem item, int quantityInTransaction)
        {
            this.shop = shop;
            this.item = item;
            nameField.text = item.GetName();
            availabilityField.text = item.GetAvailability().ToString();
            priceField.text = $"${item.GetPrice():N2}";
            icon.sprite = item.GetIcon();
            quantityField.text = quantityInTransaction.ToString();
        }

        // called by UI button (Unity Event)
        public void Plus()
        {
            shop.AddToTransaction(item.GetInventoryItem(), 1);
        }

        // called by UI button (Unity Event)
        public void Minus()
        {
            shop.SubtractFromTransaction(item.GetInventoryItem(), 1);
        }
    }
}