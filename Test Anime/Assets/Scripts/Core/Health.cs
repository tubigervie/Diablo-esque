using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float healthPoints = 100f;
        bool isDead;

        public object CaptureState()
        {
            return healthPoints;
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float)state;
            if (healthPoints == 0 && !isDead)
            {
                Die();
            }
            else if(healthPoints > 0)
            {
                isDead = false;
                GetComponent<Animator>().ResetTrigger("die");
                GetComponent<Animator>().Play("Locomotion");
            }
        }

        public void TakeDamage(float damage)
        {
            healthPoints -= damage;
            healthPoints = Mathf.Max(healthPoints, 0);
            if(healthPoints == 0 && !isDead)
            {
                Die();
            }
        }

        private void Die()
        {
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}

