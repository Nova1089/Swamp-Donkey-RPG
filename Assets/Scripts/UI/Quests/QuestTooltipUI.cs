using RPG.Quests;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestTooltipUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI questTitle;
        [SerializeField] private Transform objectiveList;
        [SerializeField] private GameObject completedObjectivePrefab;
        [SerializeField] private GameObject incompleteObjectivePrefab;

        public void Setup(QuestStatus questStatus)
        {
            if (questStatus == null) return;
            Quest quest = questStatus.GetQuest();
            questTitle.text = quest.GetTitle();
            objectiveList.DetachChildren();            
            foreach (string objective in quest.GetObjectives())
            {
                GameObject objectiveInstance;
                if (questStatus.IsObjectiveCompleted(objective))
                {
                    objectiveInstance = Instantiate(completedObjectivePrefab, objectiveList);
                }
                else
                {
                    objectiveInstance = Instantiate(incompleteObjectivePrefab, objectiveList);
                }
                
                TextMeshProUGUI textComponent = objectiveInstance.GetComponentInChildren<TextMeshProUGUI>();
                textComponent.text = objective;
            }
        }
    }
}