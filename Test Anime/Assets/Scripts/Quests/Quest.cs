using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Questing
{
    [System.Serializable]
    public class Quest
    {
        public string uniqueName;
        public string displayName;
        public string questDescription;
        public int rewardCoin;
        public int rewardXP;
        public InventoryItem itemReward;
        public List<Objective> questObjectives;
    }
}

