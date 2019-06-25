﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Resource;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
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
            if(currentWeapon == null)
            {
                EquipWeapon(defaultWeapon);
            }
        }

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
            weapon.Spawn(rightHandTransform, leftHandTransform, GetComponent<Animator>());
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
            combatTarget = target.GetComponent<Health>();
            if (combatTarget.IsDead()) Cancel();
            actionScheduler.StartAction(this);
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
            {
                return;
            }
            Health healthComponent = combatTarget.GetComponent<Health>();
            if (healthComponent.IsDead())
            {
                Cancel();
                return;
            }
            if(currentWeapon.HasProjectile())
            {
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, healthComponent, gameObject);
            }
            else
                healthComponent.TakeDamage(gameObject, currentWeapon.GetWeaponDamage());
        }

        void Shoot()
        {
            Hit();
        }

        void AttackBehavior()
        {
            transform.LookAt(combatTarget.transform);
            if(timeSinceLastAttack > currentWeapon.GetWeaponTimeBetween() && Input.GetButtonDown("Fire2"))
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

        public object CaptureState()
        {
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string) state;
            Weapon weapon = Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
            currentWeapon = weapon;
        }

    }
}
