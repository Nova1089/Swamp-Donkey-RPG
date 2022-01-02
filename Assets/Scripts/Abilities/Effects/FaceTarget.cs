using System;
using UnityEngine;

namespace RPG.Abilities
{
    [CreateAssetMenu(fileName = "Face Target", menuName = "Abilities/Effects/Face Target", order = 0)]
    public class FaceTarget : EffectStrategy
    {
        public override void StartEffect(AbilityData data, Action finished)
        {
            Transform userTransform = data.GetUser().transform;
            userTransform.LookAt(data.GetTargetedPoint());
            finished();
        }
    }
}