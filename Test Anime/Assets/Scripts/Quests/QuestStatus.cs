using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Questing
{
    [System.Serializable]
    public class QuestStatus
    {
        Quest quest;
        List<string> completedObjectives = new List<string>();

        public QuestStatus(Quest quest)
        {
            this.quest = quest;
        }

        public Quest GetQuest()
        {
            return quest;
        }

        public int GetCompletedCount()
        {
            return completedObjectives.Count;
        }

        public bool IsObjectiveComplete(string objective)
        {
            return completedObjectives.Contains(objective);
        }
    }
}

