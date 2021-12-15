using GameDevTV.Core.UI.Tooltips;
using RPG.Quests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestTooltipSpawner : TooltipSpawner
    {
        // cached references
        QuestItemUI questItemUI;

        void Awake()
        {
            questItemUI = GetComponent<QuestItemUI>();
        }

        public override bool CanCreateTooltip()
        {
            return true;
        }

        public override void UpdateTooltip(GameObject tooltip)
        {
            QuestStatus questStatus = questItemUI.GetQuestStatus();
            tooltip.GetComponent<QuestTooltipUI>().Setup(questStatus);
        }
    }
}
