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
        Health combatTarget;
        Mover mover;
        Animator anim;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;
        float timeSinceLastAttack = Mathf.Infinity;

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

            if (combatTarget.IsDead())
                return;

            if(combatTarget != null && !GetIsInRange())
            {
                mover.MoveTo(combatTarget.transform.position);
            }
            else
            {
                mover.Cancel();
                AttackBehavior();
            }
        }

        public void Attack(GameObject target)
        {
            actionScheduler.StartAction(this);
            combatTarget = target.GetComponent<Health>(); 
        }

        public void Cancel()
        {
            StopAttack();
            combatTarget = null;
        }

        private void StopAttack()
        {
            anim.ResetTrigger("attack");
            anim.SetTrigger("stopAttack");
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, combatTarget.transform.position) < weaponRange;
        }

        void Hit()
        {
            if (combatTarget == null)
                return;
            Health healthComponent = combatTarget.GetComponent<Health>();
            healthComponent.TakeDamage(weaponDamage);
        }

        void AttackBehavior()
        {
            transform.LookAt(combatTarget.transform);
            if(timeSinceLastAttack > timeBetweenAttacks)
            {
                TriggerAttack(); //This will trigger the Hit() event
                timeSinceLastAttack = 0;
            }
        }

        private void TriggerAttack()
        {
            anim.ResetTrigger("stopAttack");
            anim.SetTrigger("attack");
        }

        public bool CanAttack(GameObject target)
        {
            if (target == null)
                return false;
            Health test = target.GetComponent<Health>();
            return test != null && !test.IsDead();
        }
    }
}
