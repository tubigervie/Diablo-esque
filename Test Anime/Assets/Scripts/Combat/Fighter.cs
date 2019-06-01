using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Resources;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        ActionScheduler actionScheduler;
        Health combatTarget;
        Mover mover;
        Animator anim;

        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon;

        float timeSinceLastAttack = Mathf.Infinity;
        Weapon currentWeapon = null;

        private void Start()
        {
            mover = GetComponent<Mover>();
            anim = GetComponent<Animator>();
            actionScheduler = GetComponent<ActionScheduler>();
            EquipWeapon(defaultWeapon);
        }

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
            weapon.Spawn(rightHandTransform, leftHandTransform, anim);
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
            return Vector3.Distance(transform.position, combatTarget.transform.position) < currentWeapon.GetWeaponRange();
        }

        void Hit()
        {
            if (combatTarget == null)
                return;
            Health healthComponent = combatTarget.GetComponent<Health>();
            if(currentWeapon.HasProjectile())
            {
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, healthComponent);
            }
            else
                healthComponent.TakeDamage(currentWeapon.GetWeaponDamage());
        }

        void Shoot()
        {
            Hit();
        }

        void AttackBehavior()
        {
            transform.LookAt(combatTarget.transform);
            if(timeSinceLastAttack > currentWeapon.GetWeaponTimeBetween())
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
