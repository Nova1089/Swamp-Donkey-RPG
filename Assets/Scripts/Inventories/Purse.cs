using GameDevTV.Inventories;
using GameDevTV.Saving;
using System;
using UnityEngine;

namespace RPG.Inventories
{
    public class Purse : MonoBehaviour, ISaveable, IItemStore
    {
        // events
        public event Action OnChange;

        // configs
        [SerializeField] private float startingBalance = 400f;

        // state
        float balance = 0;

        void Awake()
        {
            balance = startingBalance;
        }

        public float GetBalance()
        {
            return balance;
        }

        public void UpdateBalance(float amount)
        {
            balance += amount;

            if (OnChange != null)
            {
                OnChange();
            }            
        }

        public object CaptureState()
        {
            return balance;
        }

        public void RestoreState(object state)
        {
            balance = (float)state;
        }

        public int AddItems(InventoryItem item, int number)
        {
            if (item is CurrencyItem)
            {
                UpdateBalance(item.GetPrice() * number);
                return number;
            }
            return 0;
        }
    }
}
