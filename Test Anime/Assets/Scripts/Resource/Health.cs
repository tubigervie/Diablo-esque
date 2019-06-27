using RPG.Control;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Resource
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float currentHealthPoints;
        float maxHealthPoints = -1f;
        [SerializeField] DamageTextSpawner damageTextSpawner;
        bool isDead;

        void Awake()
        {
            damageTextSpawner = FindObjectOfType<DamageTextSpawner>();
        }

        void Start()
        {
            if(maxHealthPoints < 0)
            {
                maxHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
            }
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
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

        public void TakeDamage(GameObject instigator, float damage)
        {
            damageTextSpawner.Create(damage, transform.position);
            currentHealthPoints -= damage;
            AIController ai = GetComponent<AIController>();
            if (ai != null)
                ai.AddAggro(damage);
            currentHealthPoints = Mathf.Max(currentHealthPoints, 0);
            if(currentHealthPoints == 0 && !isDead)
            {
                Die();
                AwardExperience(instigator);
            }
        }

        public void Heal(float amount)
        {
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
            maxHealthPoints = health;
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
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}

