using RPG.Control;
using RPG.Resource;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class SpinAttackBehaviour : AbilityBehaviour
    {
        GameObject particleObject;

        public override void Use(GameObject target = null)
        {
            if(!inUse)
            {
                GetComponent<Fighter>().Cancel();
                PlayAbilitySound();
                StartCoroutine("AttackOverTime");
                PlayParticleEffect();
                PlayAbilityAnimation();
                inUse = true;
            }
        }

        protected override void PlayParticleEffect()
        {
            var particlePrefab = config.GetParticlePrefab();
            if (particlePrefab == null) return;
            Vector3 target = transform.position;
            target.y += 1f;
            particleObject = Instantiate(
                particlePrefab,
                target,
                particlePrefab.transform.rotation
            );
            particleObject.transform.parent = transform; // set world space in prefab if required
            particleObject.GetComponent<ParticleSystem>().Play();
        }

        public override void Cancel()
        {
            inUse = false;
            Destroy(particleObject);
            GetComponent<Animator>().SetBool("abilityLoop", false);
            GetComponent<Fighter>().timeSinceLastAttack = 0;
            GetComponent<Animator>().SetBool("inBattle", true);
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

            clipOverrides[DEFAULT_LOOP_START] = config.GetAbilityAnimation();
            clipOverrides[DEFAULT_LOOP] = (config as SpinAttackConfig).GetAbilityLoop();
            clipOverrides[DEFAULT_LOOP_END] = (config as SpinAttackConfig).GetAbilityEnd();
            animatorOverrideController.ApplyOverrides(clipOverrides);

            animator.SetBool("abilityLoop", true);
            GetComponent<Fighter>().timeSinceLastAttack = 0;
            animator.SetBool("inBattle", true);
        }

        IEnumerator AttackOverTime()
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            while (true)
            {
                audioSource.PlayOneShot((config as SpinAttackConfig).GetRandomAbilitySound());
                DealRadialDamage();
                yield return new WaitForSeconds(1 / (config as SpinAttackConfig).GetHitsPerSecond());
            }
        }

        public void StopLoopAttack()
        {
            StopAllCoroutines();
        }

        private void DealRadialDamage()
        {
            // Static sphere cast for targets
            RaycastHit[] hits = Physics.SphereCastAll(
                transform.position,
                (config as SpinAttackConfig).GetRadius(),
                Vector3.up,
                (config as SpinAttackConfig).GetRadius()
            );
            bool hasHit = false;
            foreach (RaycastHit hit in hits)
            {
                var damageable = hit.collider.gameObject.GetComponent<Health>();
                bool hitPlayer = hit.collider.gameObject.GetComponent<PlayerController>();
                if (damageable != null && !hitPlayer)
                {
                    hasHit = true;
                    float damageToDeal = (config as SpinAttackConfig).GetDamageToEachTarget(GetComponent<Fighter>().GetDamage()) - hit.collider.gameObject.GetComponent<Fighter>().GetDefense(); //replace with just GetStat once weapons stats are in
                    bool shouldCrit = GetComponent<Fighter>().ShouldCrit();
                    if (shouldCrit)
                        damageToDeal *= 1.5f;
                    damageable.TakeDamage(this.gameObject, damageToDeal, shouldCrit);
                }
            }
            if(hasHit)
                GetComponent<AudioSource>().PlayOneShot((config as SpinAttackConfig).GetAttackClip());
        }
    }
}

