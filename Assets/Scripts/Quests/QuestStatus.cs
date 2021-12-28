using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestStatus
    {
        // state
        Quest quest;
        private List<string> completedObjectives = new List<string>();

        // constructors
        public QuestStatus(object objectState)
        {
            QuestStatusRecord record = objectState as QuestStatusRecord;
            this.quest = Quest.GetByName(record.questName);
            this.completedObjectives = record.completedObjectives;
        }

        // nested classes
        [System.Serializable] class QuestStatusRecord
        {
            public string questName;
            public List<string> completedObjectives;

            public QuestStatusRecord(string questName, List<string> completedObjectives)
            {
                this.questName = questName;
                this.completedObjectives = completedObjectives;
            }
        }

        // methods
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
            foreach (Quest.Objective objective in quest.GetObjectives())
            {
                foreach (string completedObjective in completedObjectives)
                {
                    if (objective.id == completedObjective)
                    {
                        totalProgress += 1;
                    }
                }
            }
            return $"{totalProgress} / {quest.GetObjectiveCount()}";
        }

        public bool IsComplete()
        {
            foreach (Quest.Objective objective in quest.GetObjectives())
            {
                if (!IsObjectiveCompleted(objective.id))
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsObjectiveCompleted(string objectiveToTest)
        {
            return completedObjectives.Contains(objectiveToTest);
        }

        public void CompleteObjective(string objective)
        {
            if (!quest.HasObjective(objective)) return;
            completedObjectives.Add(objective);
        }

        public object CaptureState()
        {
            return new QuestStatusRecord(quest.name, this.completedObjectives);
        }
    }
}
