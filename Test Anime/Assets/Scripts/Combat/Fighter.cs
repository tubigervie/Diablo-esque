using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        //Health health;
        ActionScheduler actionScheduler;
        Transform combatTarget;
        Mover mover;
        Animator anim;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;
        float timeSinceLastAttack = 0;

        private void Start()
        {
            mover = GetComponent<Mover>();
            anim = GetComponent<Animator>();
            //health = GetComponent<Health>();
            actionScheduler = GetComponent<ActionScheduler>();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (combatTarget == null)
                return;

            if(combatTarget != null && !GetIsInRange())
            {
                mover.MoveTo(combatTarget.position);
            }
            else
            {
                mover.Cancel();
                AttackBehavior();
            }
        }

        public void Attack(CombatTarget target)
        {
            actionScheduler.StartAction(this);
            combatTarget = target.transform; 
        }

        public void Cancel()
        {
            combatTarget = null;
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, combatTarget.position) < weaponRange;
        }

        void Hit()
        {
            Health healthComponent = combatTarget.GetComponent<Health>();
            healthComponent.TakeDamage(weaponDamage);
        }

        void AttackBehavior()
        {
            if(timeSinceLastAttack > timeBetweenAttacks)
            {
                anim.SetTrigger("attack"); //This will trigger the Hit() event
                timeSinceLastAttack = 0;
            }
        }
    }
}
