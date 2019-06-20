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
        [SerializeField] float maxHealthPoints = 100f;
        bool isDead;

        public object CaptureState()
        {
            return currentHealthPoints;
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void RestoreState(object state)
        {
            maxHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
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

        public float GetCurrentHealth()
        {
            return currentHealthPoints;
        }

        public float GetTotalHealth()
        {
            return maxHealthPoints;
        }

        void AwardExperience(GameObject instigator)
        {
            BaseStats enemyStats = GetComponent<BaseStats>();
            BaseStats stats = instigator.GetComponent<BaseStats>();
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;
            int prevLevel = stats.GetLevel();
            experience.GainExperience(enemyStats.GetStat(Stat.ExperienceReward));
            int currLevel = stats.GetLevel();
            if (prevLevel != currLevel)
            {
                instigator.GetComponent<Health>().maxHealthPoints = stats.GetStat(Stat.Health);
            }
        }

        private void Die()
        {
            isDead = true;
            Debug.Log(gameObject.name);
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}

