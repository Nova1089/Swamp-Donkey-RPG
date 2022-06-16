using UnityEngine;
using RPG.Movement;

namespace RPG.Control
{
    public class Collector : MonoBehaviour
    {
        // configs
        [SerializeField] float pickupRange = 2f;

        // state
        ClickablePickup targetPickup;

        // cached references
        Mover mover;

        private void Awake()
        {
            mover = GetComponent<Mover>();
        }

        private void Update()
        {            
            if (IsWithinPickupRange())
            {
                mover.Cancel();
                targetPickup.Pickup();
                targetPickup = null;
            }
        }

        private bool IsWithinPickupRange()
        {
            if (targetPickup == null) return false;
            if (Vector3.Distance(targetPickup.transform.position, transform.position) <= pickupRange)
            {
                return true;
            }
            return false;
        }

        public void Pickup(ClickablePickup pickup)
        {
            if (pickup == null) return;
            targetPickup = pickup;
            if (!IsWithinPickupRange())
            {
                mover.StartMoveAction(pickup.transform.position, 1f);
            }            
        }
    }
}

