using GameDevTV.Inventories;
using RPG.Attributes;
using RPG.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities
{
    [CreateAssetMenu(fileName = "Ability", menuName = "Abilities/Ability", order = 0)]
    public class Ability : ActionItem
    {
        // configs
        [SerializeField] private int manaCost;
        [SerializeField] private float cooldownTime = 2f;
        [SerializeField] private TargetingStrategy targetingStrategy;
        [SerializeField] private FilterStrategy[] filterStrategies;
        [SerializeField] private EffectStrategy[] effectStrategies;

        // cached references
        CooldownStore cooldownStore;
        Mana mana;

        public override void Use(GameObject user)
        {
            mana = user.GetComponent<Mana>();
            if (manaCost > mana.GetCurrentMana()) return;

            cooldownStore = user.GetComponent<CooldownStore>();
            if (cooldownStore == null) return;
            if (cooldownStore.GetTimeRemaining(this) > 0) return;
            
            AbilityData data = new AbilityData(user);

            ActionScheduler actionScheduler = user.GetComponent<ActionScheduler>();
            actionScheduler.StartAction(data);

            targetingStrategy.StartTargeting(data, 
                () => TargetAcquired(data));
        }

        private void TargetAcquired(AbilityData data)
        {
            if (data.IsCancelled()) return;
            if (!mana.UseMana(manaCost)) return;
            cooldownStore.StartCooldown(this, cooldownTime);

            foreach (var filterStrategy in filterStrategies)
            {
                data.SetTargets(filterStrategy.Filter(data.GetTargets()));
            }
             
            foreach (var effect in effectStrategies)
            {
                effect.StartEffect(data, EffectFinished);
            }            
        }

        private void EffectFinished()
        {
            
        }
    }
}