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

        // state
        private static Dictionary<string, Quest> masterQuestDictionary;

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
            if (masterQuestDictionary == null)
            {
                masterQuestDictionary = new Dictionary<string, Quest>();
                foreach (Quest quest in Resources.LoadAll<Quest>(""))
                {
                    if (masterQuestDictionary.ContainsKey(quest.name)) Debug.Log($"There are two {quest.name} quests in the system");
                    else masterQuestDictionary.Add(quest.name, quest);
                }
            }
            return masterQuestDictionary.ContainsKey(questName) ? masterQuestDictionary[questName] : null;
        }
    }
}
