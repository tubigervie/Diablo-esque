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
            public float[] health;
        }

        public float GetHealth(CharacterClass cClass, int level)
        {
            for(int i = 0; i < characterClasses.Length; i++)
            {
                if (cClass == characterClasses[i].characterClass)
                    return characterClasses[i].health[level - 1];
            }
            return 0;
        }    
    }
}

