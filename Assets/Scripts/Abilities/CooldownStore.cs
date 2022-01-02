using GameDevTV.Inventories;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities
{
    public class CooldownStore : MonoBehaviour
    {
        Dictionary<InventoryItem, float> cooldownTimers = new Dictionary<InventoryItem, float>();
        Dictionary<InventoryItem, float> cooldownAmounts = new Dictionary<InventoryItem, float>();

        void Update()
        {
            List<InventoryItem> abilities = new List<InventoryItem>(cooldownTimers.Keys);

            foreach (var ability in abilities)
            {
                cooldownTimers[ability] -= Time.deltaTime;
                if (cooldownTimers[ability] <= 0)
                {
                    cooldownTimers.Remove(ability);
                    cooldownAmounts.Remove(ability);
                }
            }
        }

        public void StartCooldown(InventoryItem ability, float cooldownTime)
        {
            cooldownTimers[ability] = cooldownTime;
            cooldownAmounts[ability] = cooldownTime;
        }

        public float GetTimeRemaining(InventoryItem ability)
        {
            if (!cooldownTimers.ContainsKey(ability)) return 0;
            return cooldownTimers[ability];
        }

        public float GetFractionRemaining(InventoryItem ability)
        {
            if (ability == null) return 0;
            if (!cooldownTimers.ContainsKey(ability)) return 0;
            return cooldownTimers[ability] / cooldownAmounts[ability];
        }
    }
}