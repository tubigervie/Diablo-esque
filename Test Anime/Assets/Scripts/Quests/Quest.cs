using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Questing
{
    [CreateAssetMenu(fileName = "Quest", menuName = "RPG/Quest", order = 0)]
    public class Quest : ScriptableObject
    {
        [SerializeField] List<Objective> objectives = new List<Objective>();
        public string uniqueName;
        public string displayName;
        public string questDescription;
        public int rewardCoin;
        public int rewardXP;
        public InventoryItem itemReward;
        [SerializeField] List<Reward> rewards = new List<Reward>();

        [System.Serializable]
        public class Reward
        {
            [Min(1)]
            public int number;
            public InventoryItem item;
        }

        [System.Serializable]
        public class Objective
        {
            public string reference;
            public string description;
        }

        public string GetTitle()
        {
            return name;
        }

        public int GetObjectiveCount()
        {
            return objectives.Count;
        }

        public IEnumerable<Reward> GetRewards()
        {
            return rewards;
        }

        public IEnumerable<Objective> GetObjectives()
        {
            return objectives;
        }

        public bool HasObjective(string objectiveRef)
        {
            foreach(var objective in objectives)
            {
                if (objective.reference == objectiveRef)
                    return true;
            }
            return false;
        }

        public static Quest GetQuestByName(string questName)
        {
            foreach (Quest quest in Resources.LoadAll<Quest>(""))
            {
                if (quest.name == questName)
                    return quest;
            }
            return null;
        }
    }
}

