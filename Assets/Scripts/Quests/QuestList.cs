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
            QuestStatus newQuestStatus = new QuestStatus(quest);
            if (statuses.Contains(newQuestStatus)) return;           
            statuses.Add(newQuestStatus);
            if (OnUpdate != null)
            {
                OnUpdate();
            }            
        }

        public IEnumerable<QuestStatus> GetStatusus()
        {
            return statuses;
        }
    }
}
