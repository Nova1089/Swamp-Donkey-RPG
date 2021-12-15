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
            title.text = questStatus.GetQuest().GetTitle();
            progress.text = questStatus.GetProgress();
        }

        public QuestStatus GetQuestStatus()
        {
            return questStatus;
        }
    }
}
