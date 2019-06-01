using RPG.Control;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float currentHealthPoints;
        [SerializeField] float maxHealthPoints = 100f;
        bool isDead;

        private void Start()
        {
            maxHealthPoints = GetComponent<BaseStats>().GetHealth();
        }


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

        public void TakeDamage(float damage)
        {
            currentHealthPoints -= damage;
            AIController ai = GetComponent<AIController>();
            if (ai != null)
                ai.AddAggro(damage);
            currentHealthPoints = Mathf.Max(currentHealthPoints, 0);
            if(currentHealthPoints == 0 && !isDead)
            {
                Die();
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

        private void Die()
        {
            isDead = true;
            Debug.Log(gameObject.name);
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}

