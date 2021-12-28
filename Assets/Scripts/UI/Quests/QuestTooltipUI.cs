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
        [SerializeField] private TextMeshProUGUI rewardsText;
        [SerializeField] private Transform objectiveList;
        [SerializeField] private GameObject completedObjectivePrefab;
        [SerializeField] private GameObject incompleteObjectivePrefab;      

        public void Setup(QuestStatus questStatus)
        {
            if (questStatus == null)
            {
                Debug.Log("Quest status is null in QuestTooltipUI");
                return;
            }
            Quest quest = questStatus.GetQuest();
            questTitle.text = quest.GetTitle();
            UpdateRewardsText(quest);
            foreach (Transform item in objectiveList)
            {
                Destroy(item.gameObject);
            }
            foreach (var objective in quest.GetObjectives())
            {
                GameObject objectiveInstance;
                if (questStatus.IsObjectiveCompleted(objective.id))
                {
                    objectiveInstance = Instantiate(completedObjectivePrefab, objectiveList);
                }
                else
                {
                    objectiveInstance = Instantiate(incompleteObjectivePrefab, objectiveList);
                }
                
                TextMeshProUGUI textComponent = objectiveInstance.GetComponentInChildren<TextMeshProUGUI>();
                textComponent.text = objective.description;
            }
        }

        private void UpdateRewardsText(Quest quest)
        {
            string textToDisplay = null;
            foreach (Quest.Reward reward in quest.GetRewards())
            {
                if (textToDisplay != null)
                {
                    textToDisplay += ", ";
                }
                if (reward.number > 1)
                {
                    textToDisplay += reward.number + " ";
                }
                textToDisplay += reward.item.GetDisplayName();
            }
            if (textToDisplay == null)
            {
                textToDisplay = "No reward";
            }
            textToDisplay += ".";
            rewardsText.text = textToDisplay;
        }
    }
}