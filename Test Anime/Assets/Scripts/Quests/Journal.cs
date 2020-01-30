using RPG.Dialogue;
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
        [SerializeField] List<Quest> completedQuests = new List<Quest>();

        public event Action journalChanged = delegate { };

        Dictionary<Quest, Voice> npcQuestMap = new Dictionary<Quest, Voice>();

        public void AddQuest(Quest quest, Voice voice = null)
        {
            if (HasCompletedQuest(quest)) return;
            npcQuestMap.Add(quest, voice);
            activeQuests.Add(quest);
            journalChanged();
        }

        public void CompleteQuest(Quest quest)
        {
            if(npcQuestMap[quest] != null)
            {
                npcQuestMap[quest].UpdateDialogueComplete();
            }
            npcQuestMap.Remove(quest);
            activeQuests.Remove(quest);
            completedQuests.Add(quest);
            journalChanged();
        }

        public bool IsActiveQuest(Quest quest)
        {
            return activeQuests.Contains(quest);
        }

        public Quest GetQuestById(string questID)
        {
            return questList.GetQuestByID(questID);
        }

        public bool HasCompletedQuest(Quest quest)
        {
            return completedQuests.Contains(quest);
        }
    }
}

