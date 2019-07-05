using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Resource;
using RPG.Stats;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : EquippableItem
    {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] DamageRange weaponDamage = new DamageRange(-1, -1);

        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] bool isRight = true;
        [SerializeField] Projectile projectile = null;
        [SerializeField] AudioClip[] audioClips;

        const string weaponName = "Weapon";

        public void Spawn(Transform rightHandTransform, Transform leftHandTransform, Animator animator)
        {
            DestroyOldWeapon(rightHandTransform, leftHandTransform);
            if(equippedPrefab != null)
            {
                GameObject weapon;
                if(isRight)
                    weapon = Instantiate(equippedPrefab, rightHandTransform);
                else
                    weapon = Instantiate(equippedPrefab, leftHandTransform);
                weapon.name = weaponName;
            }
            if(animator != null)
            {
                var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
                if (animatorOverride != null)
                    animator.runtimeAnimatorController = animatorOverride;
                else if (overrideController != null)
                    animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
        }

        private void DestroyOldWeapon(Transform rightHandTransform, Transform leftHandTransform)
        {
            Transform oldWeapon = rightHandTransform.Find(weaponName);
            if (oldWeapon == null)
                oldWeapon = leftHandTransform.Find(weaponName);
            if (oldWeapon == null) return;
            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }

        public AudioClip GetRandomWeaponSound()
        {
            if (audioClips.Length == 0)
                return null;
            return audioClips[UnityEngine.Random.Range(0, audioClips.Length)];
        }

        public List<StatModifier> GetStatModifiers()
        {
            return statModifiers;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage, bool isCritical = false)
        {
            Projectile projInstance;
            if (isRight)
                projInstance = Instantiate(projectile, rightHand.position, Quaternion.identity);
            else
                projInstance = Instantiate(projectile, leftHand.position, Quaternion.identity);
            projInstance.SetTarget(target, instigator, calculatedDamage, isCritical);
        }

        public float GetWeaponDamage()
        {
            return weaponDamage.RandomlyChooseDamage();
        }

        public DamageRange GetDamageRange()
        {
            return weaponDamage;
        }


        public float GetWeaponRange()
        {
            return weaponRange;
        }

        public float GetStatBonus(Stat bonus, BonusType type)
        {
            foreach(StatModifier modifier in statModifiers)
            {
                if (modifier.stat == bonus && modifier.bonusType == type)
                    return modifier.amount;
            }
            return 0;
        }

        public float GetWeaponTimeBetween()
        {
            return timeBetweenAttacks;
        }

        public AnimatorOverrideController GetOverride()
        {
            return animatorOverride;
        }
    }
}

