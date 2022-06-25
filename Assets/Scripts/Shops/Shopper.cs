using System;
using UnityEngine;
using RPG.Movement;

namespace RPG.Shops
{
    public class Shopper : MonoBehaviour
    {
        // configs
        [SerializeField] float shopRange = 2f;

        // state
        Shop activeShop = null;
        Shop targetShop = null;
        bool isShopping = false;

        // cached references
        Mover mover;

        // events
        public event Action OnActiveShopChanged;

        private void Awake()
        {
            mover = GetComponent<Mover>();
        }

        private void Update()
        {
            if (IsWithinShopRange() && isShopping == false)
            {                
                ActivateShop();
                isShopping = true;
            }
            else if (!IsWithinShopRange() && isShopping == true)
            {
                isShopping = false;
            }
        }

        private bool IsWithinShopRange()
        {
            if (targetShop == null) return false;

            if (Vector3.Distance(targetShop.transform.position, transform.position) <= shopRange)
            {
                return true;
            }
            return false;
        }

        public void ComeAndShop(Shop shop)
        {
            if (shop == null) return;
            targetShop = shop;

            if (!IsWithinShopRange())
            {
                mover.StartMoveAction(shop.transform.position, 1f);
            }            
        }

        public void CancelShopping()
        {
            targetShop = null;
            activeShop = null;            
            OnActiveShopChanged();
        }

        private void ActivateShop()
        {
            mover.Cancel();

            if (activeShop != null)
            {
                activeShop.SetShopper(null);
            }

            activeShop = targetShop;

            if (activeShop != null)
            {
                activeShop.SetShopper(this);
            }

            if (OnActiveShopChanged != null)
            {
                OnActiveShopChanged();
            }
        }

        public Shop GetActiveShop()
        {
            return activeShop;
        }
    }
}
