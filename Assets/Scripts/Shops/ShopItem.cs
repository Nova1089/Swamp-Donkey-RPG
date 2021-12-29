using GameDevTV.Inventories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Shops
{
    public class ShopItem
    {
        InventoryItem item;
        int availability;
        float price;
        int quantityInTransaction;

        public ShopItem(InventoryItem item, int availability, float price, int quantityInTransaction)
        {
            this.item = item;
            this.availability = availability;
            if (price > 999999.99f)
            {
                this.price = 999999.99f;
            }
            else
            {
                this.price = price;
            }            
            this.quantityInTransaction = quantityInTransaction;
        }

        public int GetAvailability()
        {
            return availability;
        }

        public Sprite GetIcon()
        {
            return item.GetIcon();
        }

        internal InventoryItem GetInventoryItem()
        {
            return item;
        }

        public float GetPrice()
        {
            if (price > 999999.99f)
            {
                return 999999.99f;
            }
            else
            {
                return price;
            }            
        }

        public string GetName()
        {
            return item.GetDisplayName();
        }

        public int GetQuantityInTransaction()
        {
            return quantityInTransaction;
        }
    }
}
