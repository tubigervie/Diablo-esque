using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public enum CharacterClass
    {
        Player, Grunt, Mage, Archer
    }

    public enum Stat
    {
        Health,
        ExperienceReward,
        ExperienceToLevelUp
    }

    public class BaseStats : MonoBehaviour
    {
        [Range(1,99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;

        public float GetStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        public int GetLevel()
        {
            Experience experience = GetComponent<Experience>();
            if (experience == null) return startingLevel;
            float currentXP = experience.GetCurrentExperience();

            int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);
            for (int levels = 1; levels <= penultimateLevel; levels++)
            {
                float XPToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, levels);
                Debug.Log("XP to level: " + XPToLevelUp + levels);
                if (XPToLevelUp > currentXP)
                    return levels;
            }
            return penultimateLevel + 1;
        }

    }

}
