using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestList : MonoBehaviour
    {
        List<QuestStatus> statuses = new List<QuestStatus>();

        // events
        public event Action OnUpdate;

        public void AddQuest(Quest quest)
        {
            if (HasQuest(quest)) return;
            QuestStatus newQuestStatus = new QuestStatus(quest);       
            statuses.Add(newQuestStatus);
            if (OnUpdate != null)
            {
                OnUpdate();
            }            
        }

        public bool HasQuest(Quest quest)
        {
            foreach (QuestStatus questStatus in statuses)
            {
                if (questStatus.GetQuest() == quest)
                {
                    return true;
                }
            }
            return false;
        }

        public IEnumerable<QuestStatus> GetStatusus()
        {
            return statuses;
        }
    }
}
