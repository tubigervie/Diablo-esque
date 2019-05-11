using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        ActionScheduler actionScheduler;
        Health combatTarget;
        Mover mover;
        Animator anim;

        [SerializeField] GameObject weaponPrefab = null;
        [SerializeField] Transform handTransform = null;

        [SerializeField] float weaponDamage = 5f;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] Weapon weapon;

        float timeSinceLastAttack = Mathf.Infinity;

        private void Start()
        {
            mover = GetComponent<Mover>();
            anim = GetComponent<Animator>();
            actionScheduler = GetComponent<ActionScheduler>();
            SpawnWeapon();
        }

        private void SpawnWeapon()
        {
            if (weapon == null) return;
            weapon.Spawn(handTransform, anim);
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
                mover.MoveTo(combatTarget.transform.position, 1f);
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
            mover.Cancel();
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
