using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Questing
{
    public class Journal : MonoBehaviour
    {
        [SerializeField] QuestList questList;

        public List<Quest> activeQuests = new List<Quest>();

        public event Action journalChanged = delegate { };

        public void AddQuest(Quest quest)
        {
            activeQuests.Add(quest);
            journalChanged();
        }

        public void CompleteQuest(Quest quest)
        {
            activeQuests.Remove(quest);
            journalChanged();
        }

        public bool IsActiveQuest(Quest quest)
        {
            return activeQuests.Contains(quest);
        }

        public Quest GetQuestById(string questID)
        {
            return null;
        }
    }
}

