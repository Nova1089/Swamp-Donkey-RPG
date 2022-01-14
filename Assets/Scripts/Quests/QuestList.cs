using GameDevTV.Inventories;
using GameDevTV.Saving;
using GameDevTV.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestList : MonoBehaviour, ISaveable, IPredicateEvaluator
    {
        List<QuestStatus> questStatuses = new List<QuestStatus>();

        // events
        public event Action OnUpdate;

        void Update()
        {
            CompleteObjectivesByPredicates();
        }

        public void AddQuest(Quest quest)
        {
            if (HasQuest(quest)) return;
            QuestStatus newQuestStatus = new QuestStatus(quest);       
            questStatuses.Add(newQuestStatus);
            if (OnUpdate != null)
            {
                OnUpdate();
            }            
        }

        public bool HasQuest(Quest quest)
        {
            return GetQuestStatus(quest) != null;
        }

        private QuestStatus GetQuestStatus(Quest quest)
        {
            foreach (QuestStatus questStatus in questStatuses)
            {
                if (questStatus.GetQuest() == quest)
                {
                    return questStatus;
                }
            }
            return null;
        }

        public IEnumerable<QuestStatus> GetStatusus()
        {
            return questStatuses;
        }

        public void CompleteObjective(Quest quest, string objective)
        {
            QuestStatus questStatus = GetQuestStatus(quest);
            if (questStatus == null) return;
            questStatus.CompleteObjective(objective);
            if (questStatus.IsComplete())
            {
                GiveReward(quest);
            }
            if (OnUpdate != null)
            {
                OnUpdate();
            }
        }

        private void GiveReward(Quest quest)
        {
            foreach (Quest.Reward reward in quest.GetRewards())
            {
                bool isSuccess = GetComponent<Inventory>().AddToFirstEmptySlot(reward.item, reward.number);
                if (!isSuccess)
                {
                    GetComponent<ItemDropper>().DropItem(reward.item, reward.number);
                }
            }
        }

        private void CompleteObjectivesByPredicates()
        {
            foreach (QuestStatus status in questStatuses)
            {
                if (status.IsComplete()) continue;

                Quest quest = status.GetQuest();
                foreach (var objective in quest.GetObjectives())
                {
                    if (status.IsObjectiveCompleted(objective.id)) continue;
                    if (!objective.usesCondition) continue;
                    if (objective.completionCondition.Check(GetComponents<IPredicateEvaluator>()))
                    {
                        CompleteObjective(quest, objective.id);
                    }
                }
            }
        }

        public object CaptureState()
        {
            List<object> state = new List<object>();
            foreach (QuestStatus questStatus in questStatuses)
            {
                state.Add(questStatus.CaptureState());
            }
            return state;            
        }

        public void RestoreState(object state)
        {
            List<object> stateList = state as List<object>;
            if (stateList == null) return;

            questStatuses.Clear();
            foreach (object objectState in stateList)
            {                
                questStatuses.Add(new QuestStatus(objectState));
            }
        }

        public bool? Evaluate(string predicate, string[] parameters)
        {

            foreach (string parameter in parameters)
            {
                switch (predicate)
                {
                    case "HasQuest":
                        return HasQuest(Quest.GetByName(parameter));
                    case "CompleteQuest":
                        Quest quest = Quest.GetByName(parameter);
                        QuestStatus questStatus = GetQuestStatus(quest);
                        if (questStatus == null) return false;
                        return questStatus.IsComplete();
                }
            }
            return null;
        }
    }
}
