using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestCompletion : MonoBehaviour
    {
        // configs
        [SerializeField] private Quest quest;
        [SerializeField] private string objective;

        // cached references
        GameObject player;
        QuestList questList;

        void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            questList = player.GetComponent<QuestList>();
        }

        public void CompleteObjective()
        {
            questList.CompleteObjective(quest, objective);
        }

        public void CompleteQuest()
        {
            questList.CompleteQuest(quest);
        }
    }
}
