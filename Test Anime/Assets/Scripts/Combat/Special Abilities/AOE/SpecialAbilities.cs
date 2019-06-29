using RPG.Saving;
using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class SpecialAbilities : MonoBehaviour, ISaveable
    {
        [SerializeField] AbilityConfig[] abilities;
        [SerializeField] Image energyBar;
        [SerializeField] float maxEnergyPoints = -1f;
        [SerializeField] float regenPointsPerSecond = 1f;
        [SerializeField] AudioClip outOfEnergy;
        [SerializeField] Image[] abilityCooldowns;
        float[] currentAbilityTimes;
        float[] coolDownTimers;
        [SerializeField] float currentEnergyPoints;
        AudioSource audioSource;
        string[] abilityNames;
        bool disableMove = false;
        [SerializeField] int skillCount = 5;
        float energyAsPercent { get { return currentEnergyPoints / maxEnergyPoints; } }

        // Use this for initialization
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            if (maxEnergyPoints < 0)
            {
                maxEnergyPoints = GetComponent<BaseStats>().GetStat(Stat.Energy);
            }

            if(abilityNames == null)
            {                    
                abilityNames = new string[abilities.Length];
                AttachInitialAbilities();
                currentAbilityTimes = new float[abilities.Length];
                coolDownTimers = new float[abilities.Length];
            }

            UpdateEnergyBar();
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().onLevelUp += RegenerateEnergy;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= RegenerateEnergy;
        }

        void Update()
        {
            if (currentEnergyPoints < maxEnergyPoints)
            {
                AddEnergyPoints();
                UpdateEnergyBar();
            }
            float t = Time.deltaTime;
            for(int i = 0; i < abilities.Length; i++)
            {
                if (coolDownTimers[i] > 0)
                {
                    coolDownTimers[i] -= t;
                    coolDownTimers[i] = Mathf.Clamp(coolDownTimers[i], 0, currentAbilityTimes[i]);
                    abilityCooldowns[i].fillAmount = coolDownTimers[i] / currentAbilityTimes[i];
                }
            }
        }

        public void SetTotalEnergy(float newTotal)
        {
            maxEnergyPoints = newTotal;
            UpdateEnergyBar();
        }

        void RegenerateEnergy()
        {
            currentEnergyPoints = maxEnergyPoints;
            UpdateEnergyBar();
        }

        void AttachInitialAbilities()
        {
            for (int abilityIndex = 0; abilityIndex < skillCount; abilityIndex++)
            {
                if (abilities[abilityIndex] == null) continue;
                abilities[abilityIndex].AttachAbilityTo(gameObject);
                abilityNames[abilityIndex] = abilities[abilityIndex].name;
            }
        }

        public void SetDisableMove(bool setter)
        {
            disableMove = setter;
        }

        public bool GetDisableMove()
        {
            return disableMove;
        }

        public void AttemptSpecialAbility(int abilityIndex, GameObject target = null)
        {
            if (abilities[abilityIndex] == null) return;

            var energyCost = abilities[abilityIndex].GetEnergyCost();

            if (energyCost <= currentEnergyPoints)
            {
                if (coolDownTimers[abilityIndex] > 0)
                    return;
                ConsumeEnergy(energyCost);
                abilities[abilityIndex].Use(target);
                disableMove = abilities[abilityIndex].GetDisableMovement();
                coolDownTimers[abilityIndex] = abilities[abilityIndex].GetCooldownTime();
                currentAbilityTimes[abilityIndex] = coolDownTimers[abilityIndex];
           
            }
            else
            {
                audioSource.PlayOneShot(outOfEnergy);
            }
        }

        public void PlayOutOfEnergy()
        {
            audioSource.PlayOneShot(outOfEnergy);
        }

        public bool CanCastSpecialAbility(int index)
        {
            return abilities[index].GetEnergyCost() <= currentEnergyPoints && coolDownTimers[index] <= 0;
        }

        public int GetNumberOfAbilities()
        {
            return abilities.Length;
        }

        private void AddEnergyPoints()
        {
            var pointsToAdd = regenPointsPerSecond * Time.deltaTime;
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints + pointsToAdd, 0, maxEnergyPoints);
        }

        public void ConsumeEnergy(float amount)
        {
            float newEnergyPoints = currentEnergyPoints - amount;
            currentEnergyPoints = Mathf.Clamp(newEnergyPoints, 0, maxEnergyPoints);
            UpdateEnergyBar();
        }

        private void UpdateEnergyBar()
        {
            if (energyBar)
            {
                energyBar.fillAmount = energyAsPercent;
            }
        }

        public object CaptureState()
        {
            return new KeyValuePair<string[], float>(abilityNames, currentEnergyPoints);
        }

        public void RestoreState(object state)
        {
            KeyValuePair<string[], float> values = (KeyValuePair<string[], float>) state;
            currentEnergyPoints = values.Value;
            abilityNames = values.Key;
            abilities = new AbilityConfig[skillCount];
            for(int i = 0; i < skillCount; i++)
            {
                if (i >= abilityNames.Length)
                    abilities[i] = null;
                abilities[i] = Resources.Load<AbilityConfig>(abilityNames[i]);
                currentAbilityTimes = new float[abilities.Length];
                coolDownTimers = new float[abilities.Length];
            }
            AttachInitialAbilities();
        }
    }

}
