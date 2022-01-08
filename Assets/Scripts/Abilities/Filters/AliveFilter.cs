using RPG.Attributes;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities.Filters
{
    [CreateAssetMenu(fileName = "Alive Filter", menuName = "Abilities/Filters/Alive", order = 0)]
    public class AliveFilter : FilterStrategy
    {
        public override IEnumerable<GameObject> Filter(IEnumerable<GameObject> objectsToFilter)
        {
            foreach (var gameObject in objectsToFilter)
            {
                if (gameObject.TryGetComponent<Health>(out Health health))
                {
                    if (!health.IsDead())
                    {
                        yield return gameObject;
                    }
                }
            }
        }
    }
}