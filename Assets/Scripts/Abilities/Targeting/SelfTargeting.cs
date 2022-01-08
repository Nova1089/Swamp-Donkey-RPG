﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities.Targeting
{

    [CreateAssetMenu(fileName = "Self Targeting", menuName = "Abilities/Targeting/Self", order = 1)]
    public class SelfTargeting : TargetingStrategy
    {
        public override void StartTargeting(AbilityData data, Action finished)
        {
            data.SetTargets(new GameObject[] { data.GetUser() });
            data.SetTargetedPoint(data.GetUser().transform.position);
            finished();
        }
    }
}