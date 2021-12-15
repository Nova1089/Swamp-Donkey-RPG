using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestGiver : MonoBehaviour
    {
        // configs
        [SerializeField] private Quest quest;

        // cached references
        GameObject player;
        QuestList questList;

        void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            questList = player.GetComponent<QuestList>();
        }

        public void GiveQuest()
        {
            questList.AddQuest(quest);
        }
    }
}
