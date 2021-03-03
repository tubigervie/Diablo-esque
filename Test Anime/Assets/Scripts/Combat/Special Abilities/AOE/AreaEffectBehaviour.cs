using RPG.Control;
using RPG.Resource;
using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class AreaEffectBehaviour : AbilityBehaviour
    {
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
            GetComponent<Fighter>().Cancel();
            PlayAbilitySound();
            DealRadialDamage();
            PlayParticleEffect();
            PlayAbilityAnimation();
        }

        private void DealRadialDamage()
        {
            // Static sphere cast for targets
            RaycastHit[] hits = Physics.SphereCastAll(
                transform.position,
                (config as AreaEffectConfig).GetRadius(),
                Vector3.up,
                (config as AreaEffectConfig).GetRadius()
            );

            foreach (RaycastHit hit in hits)
            {
                var damageable = hit.collider.gameObject.GetComponent<Health>();
                bool hitPlayer = hit.collider.gameObject.GetComponent<PlayerController>();
                if (damageable != null && !hitPlayer)
                {
                    float damageToDeal = (config as AreaEffectConfig).GetDamageToEachTarget(GetComponent<Fighter>().GetDamage()) - hit.collider.gameObject.GetComponent<BaseStats>().GetDefense(); //replace with just GetStat once weapons stats are in
                    bool shouldCrit = GetComponent<Fighter>().ShouldCrit();
                    if (shouldCrit)
                        damageToDeal *= 1.5f;
                    damageable.TakeDamage(this.gameObject, damageToDeal, shouldCrit);
                }
            }
        }
    }
}

