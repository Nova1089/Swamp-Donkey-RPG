using GameDevTV.Inventories;
using RPG.Shops;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Shops
{
    public class FilterButtonUI : MonoBehaviour
    {

        // configs
        [SerializeField] private ItemCategory category = ItemCategory.None;

        // Cached references
        Button button;
        Shop currentShop;

        void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(SelectFilter);
        }

        public void SetShop(Shop currentShop)
        {
            this.currentShop = currentShop;
        }

        private void SelectFilter()
        {
            currentShop.SelectFilter(category);
        }

        public void RefreshUI()
        {
            button.interactable = currentShop.GetCurrentFilter() != category;
        }
    }
}
