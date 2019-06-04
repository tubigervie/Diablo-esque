using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public ProgressionStat[] stats;
            //public float[] health;
        }
        
        [System.Serializable]
        class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }

        public float GetHealth(CharacterClass cClass, int level)
        {
            for(int i = 0; i < characterClasses.Length; i++)
            {
                if (cClass == characterClasses[i].characterClass)
                {
                    foreach(ProgressionStat stat in characterClasses[i].stats)
                    {
                        if(stat.stat == Stat.Health)
                            return stat.levels[level - 1];
                    }
                }
            }
            return 0;
        }


        public float GetExperience(CharacterClass cClass, int level)
        {
            for (int i = 0; i < characterClasses.Length; i++)
            {
                if (cClass == characterClasses[i].characterClass)
                {
                    foreach (ProgressionStat stat in characterClasses[i].stats)
                    {
                        if (stat.stat == Stat.ExperienceReward)
                            return stat.levels[level - 1];
                    }
                }
            }
            return 0;
        }
    }
}

