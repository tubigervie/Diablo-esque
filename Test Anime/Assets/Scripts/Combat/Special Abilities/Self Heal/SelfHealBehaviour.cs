using RPG.Control;
using RPG.Resource;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class SelfHealBehaviour : AbilityBehaviour
    {
        PlayerController player = null;

        void Start()
        {
            player = GetComponent<PlayerController>();
        }

        protected override void PlayAbilityAnimation()
        {
            var animator = GetComponent<Animator>();
            animator.SetFloat("animSpeed", config.GetAnimationSpeed());
            var animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
            animator.runtimeAnimatorController = animatorOverrideController;

            var currentOverrideController = GetComponent<Fighter>().GetOverrideController();

            AnimationClipOverrides clipOverrides = new AnimationClipOverrides(currentOverrideController.overridesCount);
            currentOverrideController.GetOverrides(clipOverrides);

            clipOverrides[DEFAULT_ABILITY_ANIMATION] = config.GetAbilityAnimation();
            animatorOverrideController.ApplyOverrides(clipOverrides);

            animator.SetBool(ABILITY_TRIGGER, false);
            animator.SetBool(ABILITY_TRIGGER, true);
            //GetComponent<Fighter>().timeSinceLastAttack = 0;
            animator.SetBool("inBattle", true);
        }

        public override void Cancel()
        {

        }

        protected override void PlayParticleEffect()
        {
            var particlePrefab = config.GetParticlePrefab();
            if (particlePrefab == null) return;
            var particleObject = Instantiate(
                particlePrefab,
                transform.position,
                particlePrefab.transform.rotation
            );
            particleObject.transform.parent = transform; // set world space in prefab if required
            particleObject.GetComponent<ParticleSystem>().Play();
        }

        public override void Use(GameObject target)
        {
            PlayAbilitySound();
            GetComponent<Fighter>().Cancel();
            var playerHealth = player.GetComponent<Health>();
            playerHealth.Heal((config as SelfHealConfig).GetExtraHealth());
            PlayParticleEffect();
            PlayAbilityAnimation();
        }
    }
}

