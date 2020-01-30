using RPG.Saving;
using RPG.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class SpecialAbilities : MonoBehaviour, ISaveable
    {
        public SpecialAbilityList abilityIndex;
        [SerializeField] AudioClip abilitySwap;
        [SerializeField] AbilityConfig[] abilities;
        [SerializeField] Image energyBar;
        [SerializeField] Text energyText;
        [SerializeField] float maxEnergyPoints = -1f;
        [SerializeField] float regenPointsPerSecond = 1f;
        [SerializeField] AudioClip outOfEnergy;
        [SerializeField] Image[] abilityCooldowns;
        [SerializeField] Image[] abilityImages;
        float[] currentAbilityTimes;
        float[] coolDownTimers;
        [SerializeField] float currentEnergyPoints;
        AudioSource audioSource;
        string[] abilityNames = null;
        bool disableMove = false;
        [SerializeField] int skillCount = 4;
        float energyAsPercent { get { return currentEnergyPoints / maxEnergyPoints; } }

        AbilityBehaviour[] abilityBehaviours;

        // Use this for initialization
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            if (maxEnergyPoints < 0)
            {
                maxEnergyPoints = GetComponent<BaseStats>().GetStat(Stat.Energy);
            }
            Debug.Log("here atleast?");
            if (abilityNames == null)
            {
                abilityNames = new string[skillCount];
                AttachInitialAbilities();
                currentAbilityTimes = new float[skillCount];
                coolDownTimers = new float[skillCount];
            }

            UpdateEnergyBar();
        }

        public void UpdateMaxEnergy()
        {
            maxEnergyPoints = GetComponent<BaseStats>().GetStat(Stat.Energy);
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints, currentEnergyPoints, maxEnergyPoints);
            UpdateEnergyBar();
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().onLevelUp += RegenerateEnergy;
            GetComponent<Inventory>().inventoryUpdated += UpdateMaxEnergy;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= RegenerateEnergy;
            GetComponent<Inventory>().inventoryUpdated -= UpdateMaxEnergy;
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

        void AttachInitialAbilities(bool isRestoringState = false)
        {
            abilityBehaviours = new AbilityBehaviour[skillCount];
            if(!isRestoringState)
            {
                Debug.Log("atleast be in here");
                abilities = new AbilityConfig[skillCount];
                for(int i = 0; i < skillCount; i++)
                {
                    abilities[i] = null;
                    abilityBehaviours[i] = null;
                }
            }
            for (int abilityIndex = 0; abilityIndex < skillCount; abilityIndex++)
            {
                if (abilities[abilityIndex] == null)
                {
                    Debug.Log("is empty");
                    abilityImages[abilityIndex].sprite = null;
                    abilityNames[abilityIndex] = "";
                    abilityImages[abilityIndex].color = Color.black;
                    continue;
                }
                abilityImages[abilityIndex].sprite = abilities[abilityIndex].GetIcon();
                abilityBehaviours[abilityIndex] = abilities[abilityIndex].AttachAbilityTo(gameObject);
                abilityNames[abilityIndex] = abilities[abilityIndex].name;
                abilityImages[abilityIndex].color = Color.white;
            }
        }

        public void SwitchAbilityAtIndex(AbilityConfig newAbility, int index)
        {
            if (abilities[index] == newAbility) return;
            for(int i = 0; i < abilities.Length; i++)
            {
                if(abilities[i] == newAbility)
                {
                    AbilityConfig abilityAtIndex = abilities[index];
                    float abilityAtIndexTimer = coolDownTimers[index];

                    float abilityFoundTimer = coolDownTimers[i];

                    abilities[index] = newAbility;
                    abilityImages[index].sprite = abilities[index].GetIcon();
                    abilityImages[index].color = Color.white;
                    abilityNames[index] = abilities[index].name;
                    currentAbilityTimes[index] = abilities[index].GetCooldownTime();
                    coolDownTimers[index] = abilityFoundTimer;
                    abilityCooldowns[index].fillAmount = coolDownTimers[index] / currentAbilityTimes[index];

                    abilities[i] = abilityAtIndex;
                    if(abilities[i] != null)
                    {
                        abilityNames[i] = abilities[i].name;
                        abilityImages[i].sprite = abilities[i].GetIcon();
                        currentAbilityTimes[i] = abilities[i].GetCooldownTime();
                        coolDownTimers[i] = abilityAtIndexTimer;
                        abilityCooldowns[i].fillAmount = coolDownTimers[i] / currentAbilityTimes[i];
                    }
                    else
                    {
                        abilityNames[i] = "";
                        abilityImages[i].sprite = null;
                        abilityImages[i].color = Color.black;
                        currentAbilityTimes[i] = 0;
                        coolDownTimers[i] = 0;
                        abilityCooldowns[i].fillAmount = 0;
                    }
                    GetComponent<AudioSource>().PlayOneShot(abilitySwap);
                    return;
                }
            }
            if(abilities[index] != null)
                abilities[index].RemoveAbilityBehavior();
            abilities[index] = newAbility;
            abilityImages[index].sprite = abilities[index].GetIcon();
            abilityImages[index].color = Color.white;
            abilities[index].AttachAbilityTo(gameObject);
            abilityNames[index] = abilities[index].name;
            currentAbilityTimes[index] = abilities[index].GetCooldownTime();
            coolDownTimers[index] = coolDownTimers[index];
            abilityCooldowns[index].fillAmount = coolDownTimers[index] / currentAbilityTimes[index];
            GetComponent<AudioSource>().PlayOneShot(abilitySwap);
        }

        public AbilityConfig GetAbilityAt(int index)
        {
            return abilities[index];
        }

        public void SetDisableMove(bool setter)
        {
            disableMove = setter;
        }

        public bool GetDisableMove()
        {
            return disableMove;
        }

        public void CancelLoops()
        {
            for(int i = 0; i < abilities.Length; i++)
            {
                if (abilities[i] != null && abilities[i].behaviour.inUse)
                {
                    abilities[i].behaviour.Cancel();
                    abilities[i].behaviour.StopAllCoroutines();
                    coolDownTimers[i] = abilities[i].GetCooldownTime();
                    currentAbilityTimes[i] = coolDownTimers[i];
                    return;
                }
            }
        }

        public void CancelAbility(int index)
        {
            for(int i = 0; i< abilities.Length; i++)
            {
                if (abilities[index] != null && abilities[i] == abilities[index] && abilities[i].behaviour.inUse)
                {
                    abilities[i].behaviour.Cancel();
                    abilities[i].behaviour.StopAllCoroutines();
                    coolDownTimers[i] = abilities[i].GetCooldownTime();
                    currentAbilityTimes[i] = coolDownTimers[i];
                    return;
                }
            }
        }

        public bool CheckIfOtherInUse(int index)
        {
            for (int i = 0; i < abilities.Length; i++)
            {
                if (abilities[i] != abilities[index]) continue;
                if (abilities[i] != null && abilities[i].behaviour.inUse)
                {
                    Debug.Log("Can't do it. " + abilities[i].name + " is being used");
                    return true;
                }
            }
            return false;
        }

        public bool IsLooping(int abilityIndex)
        {
            return abilities[abilityIndex] != null && abilities[abilityIndex].IsLooping();
        }

        public bool ContinueLoop(int abilityIndex, GameObject target = null)
        {
            var energyCost = abilities[abilityIndex].GetEnergyCost();
            if (energyCost > currentEnergyPoints)
            {
                CancelAbility(abilityIndex);
                return false;
            }
            ConsumeEnergy(energyCost);
            abilities[abilityIndex].Use(target);
            return true;
        }

        public bool AttemptSpecialAbility(int abilityIndex, GameObject target = null)
        {
            if (abilities[abilityIndex] == null) return false;
            if (coolDownTimers[abilityIndex] > 0) return false;
            var energyCost = abilities[abilityIndex].GetEnergyCost();

            if (abilities[abilityIndex].IsLooping())
            {
                ConsumeEnergy(energyCost);
                disableMove = abilities[abilityIndex].GetDisableMovement();
                abilityBehaviours[abilityIndex].Use(target);
            }
            else
            {
                ConsumeEnergy(energyCost);
                abilityBehaviours[abilityIndex].Use(target);
                disableMove = abilities[abilityIndex].GetDisableMovement();
                coolDownTimers[abilityIndex] = abilities[abilityIndex].GetCooldownTime();
                currentAbilityTimes[abilityIndex] = coolDownTimers[abilityIndex];
            }

            return true;
        }

        public void PlayOutOfEnergy()
        {
            audioSource.PlayOneShot(outOfEnergy);
        }

        public bool CanCastSpecialAbility(int index)
        {
            return abilities[index].GetEnergyCost() <= currentEnergyPoints && coolDownTimers[index] <= 0;
        }

        public bool OffCooldown(int index)
        {
            return coolDownTimers[index] <= 0;
        }

        public bool HasEnoughEnergy(int index)
        {
            return abilities[index].GetEnergyCost() <= currentEnergyPoints;
        }

        public int GetNumberOfAbilities()
        {
            return abilities.Length;
        }

        public bool AbilityInUse()
        {
            for (int i = 0; i < abilities.Length; i++)
            {
                if (abilities[i] != null && abilities[i].behaviour.inUse)
                {
                    return true;
                }
            }
            return false;
        }

        private void AddEnergyPoints()
        {
            for (int i = 0; i < abilities.Length; i++)
            {
                if (abilities[i] != null && abilities[i].behaviour.inUse) return;
            }
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
                energyText.text = "EP: " + Mathf.Round(currentEnergyPoints) + " / " + Mathf.Round(maxEnergyPoints);
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
            Debug.Log("abilityName count: " + abilityNames.Length);
            abilities = new AbilityConfig[skillCount];
            for(int i = 0; i < skillCount; i++)
            {
                abilities[i] = Resources.Load<AbilityConfig>(abilityNames[i]);
                currentAbilityTimes = new float[abilities.Length];
                coolDownTimers = new float[abilities.Length];
            }
            AttachInitialAbilities(true);
        }
    }

}
