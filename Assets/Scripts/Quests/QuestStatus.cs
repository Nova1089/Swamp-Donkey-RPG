using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestStatus
    {
         Quest quest;
        private List<string> completedObjectives = new List<string>();

        public QuestStatus(Quest quest)
        {
            this.quest = quest;
        }

        public Quest GetQuest()
        {
            return quest;
        }

        public string GetProgress()
        {            
            int totalProgress = 0;
            foreach (string objective in quest.GetObjectives())
            {
                foreach (string completedObjective in completedObjectives)
                {
                    if (objective == completedObjective)
                    {
                        totalProgress += 1;
                    }
                }
            }
            return $"{totalProgress} / {quest.GetObjectiveCount()}";
        }

        public bool IsObjectiveCompleted(string objectiveToTest)
        {
            return completedObjectives.Contains(objectiveToTest);
        }
    }
}
