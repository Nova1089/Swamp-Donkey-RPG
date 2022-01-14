using GameDevTV.Saving;
using GameDevTV.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class TraitStore : MonoBehaviour, IModifierProvider, ISaveable, IPredicateEvaluator
    {
        // configs
        [SerializeField] private TraitBonus[] bonusConfigs;

        // state
        Dictionary<Trait, int> committedPoints = new Dictionary<Trait, int>();
        Dictionary<Trait, int> stagedPoints = new Dictionary<Trait, int>();
        Dictionary<Stat, Dictionary<Trait, float>> additiveBonusCache;
        Dictionary<Stat, Dictionary<Trait, float>> percentageBonusCache;

        // unity messages
        void Awake()
        {
            additiveBonusCache = new Dictionary<Stat, Dictionary<Trait, float>>();
            percentageBonusCache = new Dictionary<Stat, Dictionary<Trait, float>>();

            foreach (var bonus in bonusConfigs)
            {
                if (!additiveBonusCache.ContainsKey(bonus.stat))
                {
                    additiveBonusCache[bonus.stat] = new Dictionary<Trait, float>();
                }
                if (!percentageBonusCache.ContainsKey(bonus.stat))
                {
                    percentageBonusCache[bonus.stat] = new Dictionary<Trait, float>();
                }

                additiveBonusCache[bonus.stat][bonus.trait] = bonus.additiveBonusPerPoint;
                percentageBonusCache[bonus.stat][bonus.trait] = bonus.percentageBonusPerPoint;
            }
        }

        public int GetStagedPoints(Trait trait)
        {
            stagedPoints.TryGetValue(trait, out int points);
            return points;
        }

        public int GetCommittedPoints(Trait trait)
        {
            committedPoints.TryGetValue(trait, out int points);
            return points;
        }

        public int GetProposedPoints(Trait trait)
        {
            return GetCommittedPoints(trait) + GetStagedPoints(trait);
        }
        public int GetTotalProposedPoints()
        {
            int total = 0;
            foreach (int points in committedPoints.Values)
            {
                total += points;
            }
            foreach (int points in stagedPoints.Values)
            {
                total += points;
            }
            return total;
        }

        public int GetUnstagedPoints()
        {
            return GetAssignablePoints() - GetTotalProposedPoints();
        }

        public int GetAssignablePoints()
        {
            return (int)GetComponent<BaseStats>().GetStat(Stat.TotalTraitPoints);
        }

        public void StagePoints(Trait trait, int points)
        {
            if (!CanStagePoints(trait, points))
            {
                return;
            }
            stagedPoints[trait] = GetStagedPoints(trait) + points;
        }

        public bool CanStagePoints(Trait trait, int points)
        {
            if (GetProposedPoints(trait) + points < GetCommittedPoints(trait)) return false;

            if (GetUnstagedPoints() < points)
            {
                return false;
            }
            return true;
        }

        public void Commit()
        {
            foreach (Trait trait in stagedPoints.Keys)
            {
                committedPoints[trait] = GetProposedPoints(trait);
            }
            stagedPoints.Clear();
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {            
            if (!additiveBonusCache.ContainsKey(stat)) yield break;

            foreach (Trait trait in additiveBonusCache[stat].Keys)
            {
                float bonus = additiveBonusCache[stat][trait];
                yield return bonus * GetCommittedPoints(trait);
            }           
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (!percentageBonusCache.ContainsKey(stat)) yield break;

            foreach (Trait trait in percentageBonusCache[stat].Keys)
            {
                float bonus = percentageBonusCache[stat][trait];
                yield return bonus * GetCommittedPoints(trait);
            }
        }

        public object CaptureState()
        {
            return committedPoints;
        }

        public void RestoreState(object state)
        {
            committedPoints = (Dictionary<Trait, int>)state;
        }

        public bool? Evaluate(string predicate, string[] parameters)
        {
            if (predicate == "MinimumTrait")
            {
                if (Enum.TryParse<Trait>(parameters[0], out Trait trait))
                {
                    return GetCommittedPoints(trait) >= Int32.Parse(parameters[1]);
                }
            }
            return null;
        }

        // classes
        [System.Serializable] class TraitBonus
        {
            public Trait trait;
            public Stat stat;
            public float additiveBonusPerPoint;
            public float percentageBonusPerPoint;
        }
    }
}