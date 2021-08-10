using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Questing
{
    [CreateAssetMenu(fileName = "Quest", menuName = "RPG/Quest", order = 0)]
    public class Quest : ScriptableObject
    {
        [SerializeField] string[] objectives;
        public string uniqueName;
        public string displayName;
        public string questDescription;
        public int rewardCoin;
        public int rewardXP;
        public InventoryItem itemReward;     

        public string GetTitle()
        {
            return name;
        }

        public int GetObjectiveCount()
        {
            return objectives.Length;
        }

        public IEnumerable<string> GetObjectives()
        {
            return objectives;
        }
    }
}

