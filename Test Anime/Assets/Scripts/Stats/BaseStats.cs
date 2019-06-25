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

        [SerializeField] int currentLevel = 0;

        private void Start()
        {
            currentLevel = GetLevel();
            Experience experience = GetComponent<Experience>();
            if (experience != null)
            {
                experience.onExperienceGained += UpdateLevel;
            }
        }

        public float GetStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        public int GetLevel()
        {
            if(currentLevel < 1)
            {
                currentLevel = CalculateLevel();
                GetComponent<Resource.Health>().SetTotalHealth(GetStat(Stat.Health));
            }
            return currentLevel;
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if(newLevel > currentLevel)
            {
                currentLevel = newLevel;
                GetComponent<Resource.Health>().SetTotalHealth(GetStat(Stat.Health));
                //play particle effects here
            }
        }

        public int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();
            if (experience == null)
            {
                Debug.Log("In here! " + gameObject.name);
                return startingLevel;
            }

            float currentXP = experience.GetCurrentExperience();

            int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);
            for (int levels = 1; levels <= penultimateLevel; levels++)
            {
                float XPToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, levels);
                Debug.Log("current: " + currentXP);
                Debug.Log("to lvl up " + XPToLevelUp);
                if (XPToLevelUp > currentXP)
                    return levels;
            }
            return penultimateLevel + 1;
        }

    }

}
