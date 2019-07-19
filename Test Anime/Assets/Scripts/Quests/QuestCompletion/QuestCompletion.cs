using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Questing
{
    public class QuestCompletion : MonoBehaviour
    {
        [SerializeField] protected string questIdToComplete;

        Quest questToComplete;

        Journal journal;

        // Start is called before the first frame update
        void Start()
        {
            journal = FindObjectOfType<Journal>();
            questToComplete = journal.GetQuestById(questIdToComplete);
        }

        protected bool IsActive()
        {
            return journal.IsActiveQuest(questToComplete);
        }

        protected void CompleteQuest()
        {
            if(IsActive())
            {
                journal.CompleteQuest(questToComplete);
                EarnReward();
            }
        }

        private void EarnReward()
        {
            var player = GameObject.FindWithTag("Player");
            player.GetComponent<Inventory>().AddCoin(questToComplete.rewardCoin);
            player.GetComponent<Experience>().GainExperience(questToComplete.rewardXP);
        }
    }
}

