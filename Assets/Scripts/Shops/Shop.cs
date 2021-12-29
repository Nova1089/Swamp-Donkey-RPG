using GameDevTV.Inventories;
using RPG.Control;
using RPG.Inventories;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Shops
{
    public class Shop : MonoBehaviour, IRaycastable
    {

        // nested classes
        [System.Serializable]
        class StockItemConfig
        {
            public InventoryItem item;
            public int initialStock;
            [Range(-300, 100)] public float buyingDiscountPercentage;
        }

        // events
        public event Action onChange;

        public string GetShopName()
        {
            return shopName;
        }

        // cached references
        GameObject player;
        Shopper currentShopper;

        // configs
        [SerializeField] private string shopName;
        [SerializeField] private StockItemConfig[] stockConfigs;

        // state
        Dictionary<InventoryItem, int> transaction = new Dictionary<InventoryItem, int>();

        void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }        

        public void SetShopper(Shopper shopper)
        {
            this.currentShopper = shopper;
        }

        public IEnumerable<ShopItem> GetFilteredItems()
        {
            return GetAllItems();
        }

        public IEnumerable<ShopItem> GetAllItems()
        {
            foreach (StockItemConfig config in stockConfigs)
            {
                float discountedPrice = config.item.GetPrice() - (config.item.GetPrice() * config.buyingDiscountPercentage / 100);
                transaction.TryGetValue(config.item, out int quantityInTransaction);
                yield return new ShopItem(config.item, config.initialStock, discountedPrice, quantityInTransaction);
            }
        }

        public void SelectCategory(ItemCategory category)
        {

        }

        public ItemCategory GetFilter()
        {
            return ItemCategory.None;
        }

        private void DisplayItems(IEnumerable<ShopItem> items)
        {

        }

        public void SwitchBuySellMode(bool isBuying)
        {

        }

        public bool IsBuyingMode()
        {
            return true;
        }
        
        public bool CanTransact()
        {
            return true;
        }

        public float TransactionTotal()
        {
            float total = 0;
            foreach (ShopItem item in GetAllItems())
            {
                total += item.GetPrice() * item.GetQuantityInTransaction();
            }
            return total;
        }

        public void AddToTransaction(InventoryItem item, int quantity)
        {
            if (!transaction.ContainsKey(item))
            {
                transaction.Add(item, quantity);
            }
            else
            {
                transaction[item] += quantity;
            }
            if (onChange != null)
            {
                onChange();
            }            
        }

        public void SubtractFromTransaction(InventoryItem item, int quantity)
        {
            if (!transaction.ContainsKey(item))
            {
                return;
            }
            else
            {
                transaction[item] -= quantity;
            }

            if (transaction[item] <= 0)
            {
                transaction.Remove(item);
            }

            if (onChange != null)
            {
                onChange();
            }
        }

        public void ConfirmTransaction()
        {
            Inventory shopperInventory = currentShopper.GetComponent<Inventory>();
            Purse shopperPurse = currentShopper.GetComponent<Purse>();
            if (shopperInventory == null || shopperPurse == null) return;

            foreach (ShopItem shopItem in GetAllItems())
            {
                InventoryItem item = shopItem.GetInventoryItem();
                int quantity = shopItem.GetQuantityInTransaction();
                float price = shopItem.GetPrice();
                for (int i = 0; i < quantity; i++)
                {                    
                    if (shopperPurse.GetBalance() < price) break;

                    bool success = shopperInventory.AddToFirstEmptySlot(item, 1);
                    if (success)
                    {
                        SubtractFromTransaction(item, 1);
                        shopperPurse.UpdateBalance(-price);
                    }
                }                
            }
        }

        private void UpdateAvailability()
        {

        }

        public void Quit()
        {
            
        }

        public CursorType GetCursorType()
        {
            return CursorType.Shop;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                callingController.GetComponent<Shopper>().SetActiveShop(this);
            }
            return true;
        }
    }
}

