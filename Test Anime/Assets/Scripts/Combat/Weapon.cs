using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Resource;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] float weaponDamage;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] bool isRight = true;
        [SerializeField] Projectile projectile = null;

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
                Debug.Log("should be in here");
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

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator)
        {
            Projectile projInstance;
            if (isRight)
                projInstance = Instantiate(projectile, rightHand.position, Quaternion.identity);
            else
                projInstance = Instantiate(projectile, leftHand.position, Quaternion.identity);
            projInstance.SetTarget(target, instigator, weaponDamage);
        }

        public float GetWeaponDamage()
        {
            return weaponDamage;
        }

        public float GetWeaponRange()
        {
            return weaponRange;
        }

        public float GetWeaponTimeBetween()
        {
            return timeBetweenAttacks;
        }
    }
}

