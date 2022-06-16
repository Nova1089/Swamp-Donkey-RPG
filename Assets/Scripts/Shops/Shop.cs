using GameDevTV.Inventories;
using GameDevTV.Saving;
using RPG.Control;
using RPG.Inventories;
using RPG.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Shops
{
    public class Shop : MonoBehaviour, IRaycastable, ISaveable
    {
        // configs
        [SerializeField] private string shopName;
        [Range(0, 100)] [SerializeField] private float sellingPercentage;
        [SerializeField] private StockItemConfig[] stockConfigs;
        [SerializeField] private float maximumBarterDiscountPercent = 80f;

        // cached references

        // state
        Dictionary<InventoryItem, int> transaction = new Dictionary<InventoryItem, int>();
        Dictionary<InventoryItem, int> stockSold = new Dictionary<InventoryItem, int>();
        Shopper currentShopper;
        bool isBuyingMode = true;
        ItemCategory currentFilter;

        // events
        public event Action onChange;

        // Unity messages

        // public methods
        public void SetShopper(Shopper shopper)
        {
            this.currentShopper = shopper;
        }

        public string GetShopName()
        {
            return shopName;
        }

        public IEnumerable<ShopItem> GetFilteredItems()
        {
            foreach (ShopItem item in GetAllItems())
            {
                if (currentFilter == ItemCategory.None || 
                    currentFilter == item.GetInventoryItem().GetCategory())
                {
                    yield return item;
                }
            }
        }

        public IEnumerable<ShopItem> GetAllItems()
        {
            Dictionary<InventoryItem, float> prices = GetPrices();
            Dictionary<InventoryItem, int> availabilities = GetAvailabilities();
            
            foreach (InventoryItem item in availabilities.Keys)
            {
                // if (config.levelToUnlock > shopperLevel) continue;
                if (availabilities[item] < 0) continue;

                float discountedPrice = prices[item];

                int quantityInTransaction = 0;
                transaction.TryGetValue(item, out quantityInTransaction);
                int availability = availabilities[item];
                yield return new ShopItem(item, availability, discountedPrice, quantityInTransaction);
            }
        }

        public void SelectFilter(ItemCategory category)
        {
            currentFilter = category;

            if (onChange != null)
            {
                onChange();
            }
        }

        public ItemCategory GetCurrentFilter()
        {
            return currentFilter;
        }

        public void SwitchBuySellMode(bool isBuying)
        {
            isBuyingMode = isBuying;

            if (onChange != null)
            {
                onChange();
            }
        }

        public bool IsBuyingMode()
        {
            return isBuyingMode;
        }
        
        public bool CanTransact()
        {
            if (IsTransactionEmpty()) return false;
            if (!HasSufficientFunds()) return false;
            if (!HasInventorySpace()) return false;

            return true;
        }

        public bool IsTransactionEmpty()
        {
            return transaction.Count == 0;
        }

        public bool HasSufficientFunds()
        {
            if (!isBuyingMode) return true;
            if (currentShopper == null) return false;
            Purse shopperPurse = currentShopper.GetComponent<Purse>();
            if (shopperPurse == null) return false;

            return shopperPurse.GetBalance() >= TransactionTotal();
        }

        public bool HasInventorySpace()
        {
            if (!isBuyingMode) return true;
            if (currentShopper == null) return false;
            Inventory shopperInventory = currentShopper.GetComponent<Inventory>();
            if (shopperInventory == null) return false;

            List<InventoryItem> itemsInTransaction = new List<InventoryItem>();
            
            foreach (var item in transaction.Keys)
            {
                for (int i = 0; i < transaction[item]; i++)
                {
                    itemsInTransaction.Add(item);
                }                
            }

            return shopperInventory.HasSpaceFor(itemsInTransaction);
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
            int currentTransactionQuantity;
            transaction.TryGetValue(item, out currentTransactionQuantity);

            var availabilities = GetAvailabilities();
            int availability = availabilities[item];

            if (availability <= 0) return;

            if (currentTransactionQuantity + quantity > availability)
            {
                transaction[item] = availability;

                if (onChange != null)
                {
                    onChange();
                }

                return;
            }

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
                    if (isBuyingMode)
                    {
                        TryBuyItem(shopperInventory, shopperPurse, item, price);
                    }
                    else
                    {
                        SellItem(shopperInventory, shopperPurse, item, price);
                    }
                }                
            }
        }

        public CursorType GetCursorType()
        {
            return CursorType.Shop;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                callingController.GetComponent<Shopper>().ComeAndShop(this);
            }
            return true;
        }

        // private methods
        private int CountInInventory(InventoryItem item)
        {
            Inventory inventory = currentShopper.GetComponent<Inventory>();
            if (inventory == null) return 0;

            int total = 0;

            for (int i = 0; i < inventory.GetSize(); i++)
            {
                if (item == inventory.GetItemInSlot(i))
                {
                    total += inventory.GetNumberInSlot(i);
                }
            }
            return total;
        }

        private Dictionary<InventoryItem, float> GetPrices()
        {
            Dictionary<InventoryItem, float> prices = new Dictionary<InventoryItem, float>();

            foreach (var config in GetAvailableConfigs())
            {
                if (!prices.ContainsKey(config.item))
                {
                    prices[config.item] = config.item.GetPrice() * GetBarterDiscount();
                }

                if (isBuyingMode)
                {
                    prices[config.item] *= (1 - config.buyingDiscountPercentage / 100);
                    continue;
                }

                prices[config.item] *= (sellingPercentage / 100);        
            }
            return prices;
        }

        private float GetBarterDiscount()
        {
            BaseStats baseStats = currentShopper.GetComponent<BaseStats>();
            float discount = baseStats.GetStat(Stat.BuyingDiscountPercentage);
            return (1 - Mathf.Min(discount, maximumBarterDiscountPercent) / 100);
        }

        private IEnumerable<StockItemConfig> GetAvailableConfigs()
        {
            int shopperLevel = GetShopperLevel();

            foreach (var config in stockConfigs)
            {
                if (config.levelToUnlock > shopperLevel) continue;
                yield return config;
            }
        }

        private Dictionary<InventoryItem, int> GetAvailabilities()
        {
            Dictionary<InventoryItem, int> availabilities = new Dictionary<InventoryItem, int>();

            foreach (var config in GetAvailableConfigs())
            {
                if (isBuyingMode)
                {
                    if (!availabilities.ContainsKey(config.item))
                    {
                        int sold = 0;
                        stockSold.TryGetValue(config.item, out sold);
                        availabilities[config.item] = -sold;
                    }
                    availabilities[config.item] += config.initialStock;
                    continue;
                }

                if (!availabilities.ContainsKey(config.item))
                {
                    availabilities[config.item] = CountInInventory(config.item);
                }
            }
            return availabilities;
        }

        private void SellItem(Inventory shopperInventory, Purse shopperPurse, InventoryItem item, float price)
        {
            for (int i = 0; i < shopperInventory.GetSize(); i++)
            {
                if (item == shopperInventory.GetItemInSlot(i))
                {
                    shopperInventory.RemoveFromSlot(i, 1);
                    break;
                }
            }

            SubtractFromTransaction(item, 1);
            shopperPurse.UpdateBalance(price);
            if (!stockSold.ContainsKey(item))
            {
                stockSold[item] = 0;
            }
            stockSold[item]--;
            onChange();
        }

        private void TryBuyItem(Inventory shopperInventory, Purse shopperPurse, InventoryItem item, float price)
        {
            if (shopperPurse.GetBalance() < price) return;

            bool success = shopperInventory.AddToFirstEmptySlot(item, 1);
            if (success)
            {
                SubtractFromTransaction(item, 1);
                shopperPurse.UpdateBalance(-price);
                if (!stockSold.ContainsKey(item))
                {
                    stockSold[item] = 0;
                }
                stockSold[item]++;
                onChange();
            }
        }

        private int GetShopperLevel()
        {
            BaseStats stats = currentShopper.GetComponent<BaseStats>();
            if (stats == null) return 0;
            return stats.GetLevel();
        }


        // interface implementations
        public object CaptureState()
        {  
            Dictionary<string, int> saveObject = new Dictionary<string, int>();

            foreach (var pair in stockSold)
            {
                saveObject[pair.Key.GetItemID()] = pair.Value;
            }
            return saveObject;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, int> saveObject = (Dictionary<string, int>)state;
            stockSold.Clear();
            foreach (var pair in saveObject)
            {
                stockSold[InventoryItem.GetFromID(pair.Key)] = pair.Value;
            }
        }

        // classes
        [System.Serializable]
        class StockItemConfig
        {
            public InventoryItem item;
            public int initialStock;
            [Range(-300, 100)] public float buyingDiscountPercentage;
            public int levelToUnlock = 0;
        }
    }
}

