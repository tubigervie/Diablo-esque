using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;
        Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;

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

        public float GetStat(Stat cStat, CharacterClass cClass, int level)
        {
            BuildLookup();
            float[] levels = lookupTable[cClass][cStat];
            if (levels.Length < level)
                return 0;
            Debug.Log("what I got: " + levels[level - 1]);
            return levels[level - 1];
        }

        public int GetLevels(Stat stat, CharacterClass characterClass)
        {
            BuildLookup();
            float[] levels = lookupTable[characterClass][stat];
            return levels.Length;
        }

        private void BuildLookup()
        {
            if (lookupTable != null) return;

            lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();
            foreach(ProgressionCharacterClass progressionClass in characterClasses)
            {
                var statLookupTable = new Dictionary<Stat, float[]>();
                foreach(ProgressionStat progStat in progressionClass.stats)
                {
                    statLookupTable[progStat.stat] = progStat.levels;
                }

                lookupTable[progressionClass.characterClass] = statLookupTable;
            }
        }
    }
}

