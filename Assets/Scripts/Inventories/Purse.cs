using System;
using UnityEngine;

namespace RPG.Inventories
{
    public class Purse : MonoBehaviour
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
    }
}
