using RPG.Quests;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestItemUI : MonoBehaviour
    {
        // references
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI progress;

        // state
        QuestStatus questStatus;

        public void Setup(QuestStatus questStatus)
        {
            this.questStatus = questStatus;
            if (this.questStatus == null)
            {
                Debug.Log("questStatus is null in QuestItemUI");
            }
            title.text = questStatus.GetQuest().GetTitle();
            progress.text = questStatus.GetProgress();
        }

        public QuestStatus GetQuestStatus()
        {
            return questStatus;
        }
    }
}
