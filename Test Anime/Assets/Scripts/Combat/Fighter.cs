using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Resource;
using RPG.Stats;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, IModifier
    {
        ActionScheduler actionScheduler;
        Health combatTarget;
        Mover mover;
        public Animator anim;

        public Transform rightHandTransform = null;
        public Transform leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon;

        public float timeSinceLastAttack = Mathf.Infinity;
        [SerializeField] WeaponInstance currentWeapon = null;

        bool attackLock = false;
        bool clickInput = false;

        private void Start()
        {
            mover = GetComponent<Mover>();
            anim = GetComponent<Animator>(); 
            actionScheduler = GetComponent<ActionScheduler>();
            if(rightHandTransform == null)
                rightHandTransform = GameObject.FindGameObjectWithTag("PlayerRightHand").transform;
            if(leftHandTransform == null)
                leftHandTransform = GameObject.FindGameObjectWithTag("PlayerLeftHand").transform;
            if (currentWeapon == null)
            {
                WeaponInstance defaultInstance = new WeaponInstance(defaultWeapon, 1);
                EquipWeapon(defaultInstance);
            }
        }

        public WeaponInstance GetCurrentWeapon()
        {
            return currentWeapon;
        }

        public void EquipWeapon(WeaponInstance weapon)
        {
            currentWeapon = weapon;
            weapon.weaponBase.Spawn(rightHandTransform, leftHandTransform, GetComponent<Animator>());
            if(GetComponent<Inventory>() != null)
            {
                if(weapon.weaponBase.name != "Unarmed")
                {
                    GetComponent<Inventory>().SetWeaponSlot(weapon);
                }
            }
            timeSinceLastAttack = Mathf.Infinity;
            if(anim != null)
            {
                anim.SetBool("inBattle", false);
            }
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
            return Vector3.Distance(transform.position, combatTarget.transform.position) < currentWeapon.weaponBase.GetWeaponRange();
        }

        public float GetDamage()
        {
            BaseStats stat = GetComponent<BaseStats>();
            float damage = stat.GetStat(Stat.Damage) + currentWeapon.GetWeaponDamage() + stat.GetStrengthDamageBonus();
            return damage;
        }

        public DamageRange GetDamageRange()
        {
            return currentWeapon.GetDamageRange();
        }

        public bool ShouldCrit()
        {
            BaseStats stat = GetComponent<BaseStats>();
            float critChance = (stat.GetStat(Stat.CriticalHitChance) + stat.GetDexCritChanceBonus()) / 100;

            return Random.value < critChance;
        }

        void Hit()
        {
            if (combatTarget == null)
            {
                return;
            }
            attackLock = true;
            float damage = Mathf.Clamp(GetDamage() - combatTarget.GetComponent<BaseStats>().GetDefense(), 0, Mathf.Infinity);

            bool shouldBeCritical = ShouldCrit();

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
            if(currentWeapon.weaponBase.HasProjectile())
            {
                AudioClip damageSound = currentWeapon.weaponBase.GetRandomWeaponSound();
                if (damageSound != null)
                    GetComponent<AudioSource>().PlayOneShot(damageSound);
                currentWeapon.weaponBase.LaunchProjectile(rightHandTransform, leftHandTransform, healthComponent, gameObject, damage, shouldBeCritical);
            }
            else
            {
                AudioClip damageSound = currentWeapon.weaponBase.GetRandomWeaponSound();
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
            if (timeSinceLastAttack > currentWeapon.weaponBase.GetWeaponTimeBetween())
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

        public IEnumerable<float> GetAdditiveModifier(Stat stat)
        {
            if (currentWeapon == null)
            {
                WeaponInstance defaultInstance = new WeaponInstance(defaultWeapon, 1);
                EquipWeapon(defaultInstance);
            }
            foreach (var modifier in currentWeapon.properties.statModifiers)
            {
                if(modifier.stat == stat)
                    yield return currentWeapon.GetStatBonus(modifier.stat, BonusType.Flat);
            }
        }

        public IEnumerable<float> GetPercentageModifier(Stat stat)
        {
            if (currentWeapon == null)
            {
                WeaponInstance defaultInstance = new WeaponInstance(defaultWeapon, 1);
                EquipWeapon(defaultInstance);
            }
            foreach (var modifier in currentWeapon.properties.statModifiers)
            {
                if (modifier.stat == stat)
                    yield return currentWeapon.GetStatBonus(modifier.stat, BonusType.Percentage);
            }
        }

        public AnimatorOverrideController GetOverrideController()
        {
            return currentWeapon.weaponBase.GetOverride();
        }

    }
}
