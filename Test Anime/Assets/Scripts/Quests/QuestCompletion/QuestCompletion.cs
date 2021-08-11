using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Questing
{
    public class QuestCompletion : MonoBehaviour
    {
        [SerializeField] protected string objective;

        [SerializeField] Quest questToComplete;
        // Start is called before the first frame update
        void Start()
        {
        }

        public void CompleteObjective()
        {
            QuestList questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            questList.CompleteObjective(questToComplete, objective);
        }

        private void EarnReward()
        {
            var player = GameObject.FindWithTag("Player");
            player.GetComponent<Inventory>().AddCoin(questToComplete.rewardCoin);
            player.GetComponent<Experience>().GainExperience(questToComplete.rewardXP);
        }
    }
}

