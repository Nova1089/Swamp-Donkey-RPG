using RPG.Quests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestListUI : MonoBehaviour
    {
        // cached references
        [SerializeField] QuestItemUI questPrefab;
        GameObject player;
        QuestList questList;

        void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            questList = player.GetComponent<QuestList>();
        }

        void OnEnable()
        {
            questList.OnUpdate += PopulateQuestList;
        }

        void Start()
        {
            PopulateQuestList();
        }

        private void PopulateQuestList()
        {
            if (questList == null) return;
            if (questPrefab == null) return;
            foreach (Transform item in this.transform)
            {
                Destroy(item.gameObject);
            }
            foreach (QuestStatus questStatus in questList.GetStatusus())
            {
                QuestItemUI questUIInstance = Instantiate(questPrefab, this.transform);
                questUIInstance.Setup(questStatus);
            }
        }
    }
}
