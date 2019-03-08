using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float healthPoints = 100f;
        bool isDead;

        public void TakeDamage(float damage)
        {
            healthPoints -= damage;
            healthPoints = Mathf.Max(healthPoints, 0);
            if(healthPoints == 0 && !isDead)
            {
                isDead = true;
                GetComponent<Animator>().SetTrigger("die");
            }
        }
    }
}

