using System.Collections;
using System.Collections.Generic;
using GameDevTV.Core.UI.Dragging;
using GameDevTV.Inventories;
using RPG.Abilities;
using UnityEngine;
using UnityEngine.UI;

namespace GameDevTV.UI.Inventories
{
    /// <summary>
    /// The UI slot for the player action bar.
    /// </summary>
    public class ActionSlotUI : MonoBehaviour, IItemHolder, IDragContainer<GameDevTV.Inventories.InventoryItem>
    {
        // CONFIG DATA
        [SerializeField] InventoryItemIcon icon = null;
        [SerializeField] int index = 0;
        [SerializeField] private Image cooldownOverlay;

        // CACHE
        ActionStore actionStore;
        CooldownStore cooldownStore;

        // LIFECYCLE METHODS
        private void Awake()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            actionStore = player.GetComponent<ActionStore>();
            actionStore.storeUpdated += UpdateIcon;
            cooldownStore = player.GetComponent<CooldownStore>();
        }

        private void Update()
        {
            cooldownOverlay.fillAmount = cooldownStore.GetFractionRemaining(GetItem());
        }

        // PUBLIC

        public void AddItems(GameDevTV.Inventories.InventoryItem item, int number)
        {
            actionStore.AddAction(item, index, number);
        }

        public GameDevTV.Inventories.InventoryItem GetItem()
        {
            return actionStore.GetAction(index);
        }

        public int GetNumber()
        {
            return actionStore.GetNumber(index);
        }

        public int MaxAcceptable(GameDevTV.Inventories.InventoryItem item)
        {
            return actionStore.MaxAcceptable(item, index);
        }

        public void RemoveItems(int number)
        {
            actionStore.RemoveItems(index, number);
        }

        // PRIVATE

        void UpdateIcon()
        {
            icon.SetItem(GetItem(), GetNumber());
        }
    }
}
