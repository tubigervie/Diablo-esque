using RPG.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Questing
{
    public class QuestList : MonoBehaviour, ISaveable
    {
        List<QuestStatus> statuses = new List<QuestStatus>();

        public event Action onUpdate;

        public IEnumerable<QuestStatus> GetStatuses()
        {
            return statuses;
        }

        public void AddQuest(Quest quest)
        {
            if (HasQuest(quest)) return;
            QuestStatus newStatus = new QuestStatus(quest);
            statuses.Add(newStatus);
            if (onUpdate != null)
                onUpdate();
        }

        public void CompleteObjective(Quest questToComplete, string objective)
        {
            QuestStatus status = GetQuestStatus(questToComplete);
            if(status != null)
            {
                status.CompleteObjective(objective);
                if(status.IsComplete())
                {
                    GiveReward(questToComplete);
                }
                if (onUpdate != null)
                    onUpdate();
            }
        }

        private void GiveReward(Quest quest)
        {
            Inventory inventory = GetComponent<Inventory>();
            foreach (var reward in quest.GetRewards())
            {
                for(int i = 0; i < reward.number; i++)
                {
                    ItemInstance newItem = new ItemInstance(reward.item, 1);
                    bool success = inventory.AddToFirstEmptySlot(newItem);
                    if(!success)
                    {
                        inventory.DropItem(newItem);
                    }
                }
            }
        }

        public bool HasQuest(Quest quest)
        {
            return GetQuestStatus(quest) != null;
        }

        private QuestStatus GetQuestStatus(Quest quest)
        {
            foreach (QuestStatus status in statuses)
            {
                if (status.GetQuest() == quest)
                {
                    return status;
                }
            }
            return null;
        }

        public object CaptureState()
        {
            List<object> state = new List<object>();
            foreach(QuestStatus status in statuses)
            {
                state.Add(status.CaptureState());
            }
            return state;
        }

        public void RestoreState(object state)
        {
            List<object> stateList = state as List<object>;
            if (stateList == null) return;

            statuses.Clear();
            foreach(object objectState in stateList)
            {
                statuses.Add(new QuestStatus(objectState));
            }
        }
    }
}

