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

        public float timeSinceLastAttack = Mathf.Infinity;
        [SerializeField] Weapon currentWeapon = null;
        bool attackLock = false;
        bool clickInput = false;

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


            if (combatTarget != null && !GetIsInRange() && !attackLock && clickInput)
            {
                mover.MoveTo(combatTarget.transform.position, 1f);
            }
            else if(clickInput)
            {
                mover.Cancel();
                AttackBehavior();
                clickInput = false;
            }
        }

        public void Attack(GameObject target)
        {
            clickInput = true;
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
            clickInput = false;
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

        public float GetDamage()
        {
            BaseStats stat = GetComponent<BaseStats>();
            float damage = stat.GetStat(Stat.Damage) + currentWeapon.GetWeaponDamage() + stat.GetAttributeBonus(Stat.Strength);
            return damage;
        }

        void Hit()
        {
            if (combatTarget == null)
            {
                return;
            }
            attackLock = true;
            float damage = GetDamage();
            BaseStats stat = GetComponent<BaseStats>();
            float critChance = (stat.GetStat(Stat.CriticalHitChance) + stat.GetAttributeBonus(Stat.Dexterity)) / 100;

            bool shouldBeCritical = Random.value < critChance;

            if (shouldBeCritical)
            {
                damage *= 1.5f;
            }

            Health healthComponent = combatTarget.GetComponent<Health>();
            if (healthComponent.IsDead())
            {
                Cancel();
                return;
            }
            if(currentWeapon.HasProjectile())
            {
                AudioClip damageSound = currentWeapon.GetRandomWeaponSound();
                if (damageSound != null)
                    GetComponent<AudioSource>().PlayOneShot(damageSound);
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, healthComponent, gameObject, damage, shouldBeCritical);
            }
            else
            {
                AudioClip damageSound = currentWeapon.GetRandomWeaponSound();
                if (damageSound != null)
                    combatTarget.GetComponent<AudioSource>().PlayOneShot(damageSound);
                healthComponent.TakeDamage(gameObject, damage, shouldBeCritical);
            }
            attackLock = false;
        }

        void Shoot()
        {
            Hit();
        }

        void AttackBehavior()
        {
            if (timeSinceLastAttack > currentWeapon.GetWeaponTimeBetween())
            {
                transform.LookAt(combatTarget.transform);
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
            return (currentWeapon != null) ? currentWeapon.name : null;     
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
            foreach(var modifier in currentWeapon.GetStatModifiers())
            {
                if(modifier.stat == stat)
                    yield return currentWeapon.GetStatBonus(modifier.stat, BonusType.Flat);
            }
        }

        public IEnumerable<float> GetPercentageModifier(Stat stat)
        {
            foreach (var modifier in currentWeapon.GetStatModifiers())
            {
                if (modifier.stat == stat)
                    yield return currentWeapon.GetStatBonus(modifier.stat, BonusType.Percentage);
            }
        }

        public AnimatorOverrideController GetOverrideController()
        {
            return currentWeapon.GetOverride();
        }

    }
}
