using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
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
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}

