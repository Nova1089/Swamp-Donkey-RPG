using RPG.Attributes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities
{
    [CreateAssetMenu(fileName = "Health Effect", menuName = "Abilities/Effects/Health", order = 0)]
    public class HealthEffect : EffectStrategy
    {
        [Tooltip("Positive to heal, negative to damage.")]
        [SerializeField] private float amountToChange;

        public override void StartEffect(AbilityData data, Action finished)
        {
            foreach (var target in data.GetTargets())
            {
                Health health = target.GetComponent<Health>();
                if (health == null) continue;

                if (amountToChange < 0)
                {
                    health.TakeDamage(data.GetUser(), -amountToChange);
                }
                else
                {
                    health.Heal(amountToChange);
                }                
            }
        }
    }
}