using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
        ExperienceToLevelUp,
        Damage,
        Energy,
        Strength,
        Dexterity,
        Constitution,
        CriticalHitChance,
        Defense
    }

    public class BaseStats : MonoBehaviour
    {
        [Range(1,99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelupParticle = null;
        [SerializeField] int currentLevel = 0;
        [SerializeField] bool shouldUseModifiers = false;
        [SerializeField] AudioClip levelUpAudio;

        [SerializeField] float damageBonusPerStrengthPoint = .5f;
        [SerializeField] float criticalHitChancePerDexterityPoint = .2f;
        [SerializeField] float defenseBonusPerConstitutionPoint = .5f;
        [SerializeField] float healthBonusPerConstitutionPoint = 2f;

        public event Action onLevelUp;

        Experience experience;

        private void Awake()
        {
            Experience experience = GetComponent<Experience>();
            if (experience != null)
            {
                experience.onExperienceGained += UpdateLevel;
                experience.onExperienceLoaded += ChangeLevel;
                experience.alreadyLoaded = true;
            }
        }

        private void Start()
        {
            currentLevel = GetLevel();
        }

        private void OnDisable()
        {
            if (experience != null)
            {
                experience.onExperienceGained -= UpdateLevel;
                experience.onExperienceLoaded -= ChangeLevel;
            }
        }

        public float GetStat(Stat stat)
        {
            float percentageBonus = (1 + GetPercentageModifier(stat) / 100);
            //Debug.Log(stat + " % bonus: " + percentageBonus);
            float additiveBonus = (GetAdditiveModifier(stat));
            //Debug.Log(stat + " + bonus: " + additiveBonus);
            float baseAmount = GetBaseStat(stat);
            //Debug.Log(stat + " base: " + baseAmount);
            //Debug.Log("Total: " + ((baseAmount * percentageBonus) + additiveBonus));
            return (baseAmount * percentageBonus) + additiveBonus;
        }


        public float GetConstitutionHealthBonus()
        {
            if (!shouldUseModifiers) return 0;
            return GetStat(Stat.Constitution) * healthBonusPerConstitutionPoint;
        }

        public float GetConstitutionDefenseBonus()
        {
            if (!shouldUseModifiers) return 0;
            return GetStat(Stat.Constitution) * defenseBonusPerConstitutionPoint;
        }


        public float GetStrengthDamageBonus()
        {
            if (!shouldUseModifiers) return 0;
            return GetStat(Stat.Strength) * damageBonusPerStrengthPoint;
        }

        public float GetDexCritChanceBonus()
        {
            if (!shouldUseModifiers) return 0;
            return GetStat(Stat.Dexterity) * criticalHitChancePerDexterityPoint;
        }

        public float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        public float GetStatByLevel(Stat stat, int level)
        {
            return progression.GetStat(stat, characterClass, level);
        }

        private float GetAdditiveModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;
            float total = 0;
            foreach(IModifier provider in GetComponents<IModifier>())
            {
                foreach(float modifier in provider.GetAdditiveModifier(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }

        private float GetPercentageModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;
            float total = 0;
            foreach(IModifier provider in GetComponents<IModifier>())
            {
                foreach( float modifier in provider.GetPercentageModifier(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }

        public int GetLevel()
        {
            if(currentLevel < 1)
            {
                currentLevel = CalculateLevel();
                GetComponent<Resource.Health>().SetTotalHealth(GetStat(Stat.Health) + GetConstitutionHealthBonus());
            }
            return currentLevel;
        }

        private void ChangeLevel()
        {
            currentLevel = CalculateLevel();
            GetComponent<Resource.Health>().SetTotalHealth(GetStat(Stat.Health) + GetConstitutionHealthBonus());
            if(characterClass == CharacterClass.Player)
                GetComponent<Combat.SpecialAbilities>().SetTotalEnergy(GetStat(Stat.Energy));
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if(newLevel > currentLevel)
            {
                currentLevel = newLevel;
                GetComponent<Resource.Health>().SetTotalHealth(GetStat(Stat.Health) + GetConstitutionHealthBonus());
                if (characterClass == CharacterClass.Player)
                    GetComponent<Combat.SpecialAbilities>().SetTotalEnergy(GetStat(Stat.Energy));
                LevelUpEffect();
                onLevelUp();
                //play particle effects here
            }
        }

        private void LevelUpEffect()
        {
            Instantiate(levelupParticle, transform);
            GetComponent<AudioSource>().PlayOneShot(levelUpAudio);
        }

        public int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();
            if (experience == null)
            {
                return startingLevel;
            }

            float currentXP = experience.GetCurrentExperience();

            int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);
            for (int levels = 1; levels <= penultimateLevel; levels++)
            {
                float XPToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, levels);
                if (XPToLevelUp > currentXP)
                    return levels;
            }
            return penultimateLevel + 1;
        }

    }

}
