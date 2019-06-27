using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Resource;
using RPG.Stats;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifier
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
        bool attackLock = false;

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
            timeSinceLastAttack = Mathf.Infinity;
            if(anim != null)
                anim.SetBool("inBattle", false);
        }


        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (timeSinceLastAttack > 5)
                anim.SetBool("inBattle", false);
            if (combatTarget == null)
                return;

            if (combatTarget.IsDead())
                return;

            if(combatTarget != null && !GetIsInRange() && !attackLock)
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
            if (combatTarget.IsDead())
            {
                Cancel();
                return;
            }
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
            attackLock = false;
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
            attackLock = true;
            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            Health healthComponent = combatTarget.GetComponent<Health>();
            if (healthComponent.IsDead())
            {
                Cancel();
                return;
            }
            if(currentWeapon.HasProjectile())
            {
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, healthComponent, gameObject, damage);
            }
            else
            {
                healthComponent.TakeDamage(gameObject, damage);
            }
            attackLock = false;
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
                if(!anim.GetBool("inBattle"))
                    anim.SetBool("inBattle", true);
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

        public IEnumerable<float> GetAdditiveModifier(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeapon.GetWeaponDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifier(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeapon.GetPercentageBonus();
            }
        }

    }
}
