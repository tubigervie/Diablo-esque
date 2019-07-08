using RPG.Control;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Resource
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float currentHealthPoints;
        [SerializeField] float maxHealthPoints = -1f;
        [SerializeField] TextNumberSpawner textNumberSpawner;
        [SerializeField] bool isDead;
        bool isRestored;
        public event Action onDie = delegate { };

    void Awake()
        {
            textNumberSpawner = FindObjectOfType<TextNumberSpawner>();
        }

        void Start()
        {
            if(maxHealthPoints < 0)
            {
                UpdateMaxHealth();
                RegenerateHealth();
            }
            isRestored = false;
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
            Inventory inventory = GetComponent<Inventory>();
            if (inventory != null)
                inventory.inventoryUpdated += UpdateMaxHealth;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
            Inventory inventory = GetComponent<Inventory>();
            if (inventory != null)
                inventory.inventoryUpdated -= UpdateMaxHealth;
        }

        public void UpdateMaxHealth()
        {
            float consBonus = GetComponent<BaseStats>().GetConstitutionHealthBonus();
            maxHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) + consBonus;
            if(!isRestored)
                currentHealthPoints = Mathf.Clamp(currentHealthPoints, currentHealthPoints, maxHealthPoints);
            //Debug.Log("Setting current health for: " + this.gameObject.name + " at " + currentHealthPoints);
        }

        public void ClampCurrentHealth()
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints, currentHealthPoints, maxHealthPoints);
        }

        public object CaptureState()
        {
            return currentHealthPoints;
        }

        public void RegenerateHealth()
        {
            currentHealthPoints = maxHealthPoints;
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void RestoreState(object state)
        {
            isRestored = true;
            currentHealthPoints = (float)state;
            if (currentHealthPoints == 0 && !isDead)
            {
                Die();
            }
            else if(currentHealthPoints > 0)
            {
                isDead = false;
                GetComponent<Animator>().ResetTrigger("die");
                GetComponent<Animator>().Play("Locomotion");
            }
        }

        public void TakeDamage(GameObject instigator, float damage, bool isCritical = false)
        {
            if (isCritical)
                textNumberSpawner.CreateCritText(damage, transform.position);
            else
                textNumberSpawner.CreateDamageText(damage, transform.position);

            currentHealthPoints -= damage;
            AIController ai = GetComponent<AIController>();
            if (ai != null)
                ai.AddAggro(damage);
            currentHealthPoints = Mathf.Max(currentHealthPoints, 0);
            if(currentHealthPoints == 0 && !isDead)
            {
                Die();
                onDie();
                AwardExperience(instigator);
            }
        }

        public void Heal(float amount)
        {
            textNumberSpawner.CreateHealText(amount, transform.position);
            currentHealthPoints = Mathf.Clamp(currentHealthPoints + amount, currentHealthPoints, maxHealthPoints);
        }

        public float GetCurrentHealth()
        {
            return currentHealthPoints;
        }

        public float GetTotalHealth()
        {
            return maxHealthPoints;
        }

        public void SetTotalHealth(float health)
        {
            float oldMax = maxHealthPoints;
            maxHealthPoints = health;
            if (oldMax < 0)
                currentHealthPoints = maxHealthPoints;
        }

        void AwardExperience(GameObject instigator)
        {
            BaseStats enemyStats = GetComponent<BaseStats>();
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;
            experience.GainExperience(enemyStats.GetStat(Stat.ExperienceReward));
        }

        private void Die()
        {
            isDead = true;
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<Animator>().SetTrigger("die");
            if (GetComponent<ActionScheduler>() != null)
                GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}

