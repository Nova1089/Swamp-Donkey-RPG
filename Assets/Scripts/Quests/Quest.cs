using GameDevTV.Inventories;
using GameDevTV.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{

    [CreateAssetMenu(fileName = "Quest", menuName = "RPG Project/Quest", order = 0)]
    public class Quest : ScriptableObject
    {
        // configs
        [SerializeField] List<Objective> objectives = new List<Objective>();
        [SerializeField] List<Reward> rewards = new List<Reward>();

        // nested classes
        [System.Serializable] public class Objective
        {
            public string id;
            public string description;
            public bool usesCondition = false;
            public Condition completionCondition;
        }

        [System.Serializable] public class Reward
        {
            [Min(1)] public int number;
            public InventoryItem item;
        }

        // methods
        public IEnumerable<Objective> GetObjectives()
        {
            return objectives;
        }

        public IEnumerable<Reward> GetRewards()
        {
            return rewards;
        }

        public string GetTitle()
        {
            return name;
        }

        public int GetObjectiveCount()
        {
            return objectives.Count;
        }

        public bool HasObjective(string objectiveID)
        {
            foreach (var objective in objectives)
            {
                if (objective.id == objectiveID)
                {
                    return true;
                }
            }
            return false;
        }

        public static Quest GetByName(string questName)
        {
            foreach (Quest quest in Resources.LoadAll<Quest>(""))
            {
                if (quest.name == questName)
                {
                    return quest;
                }
            }
            return null;
        }
    }
}
