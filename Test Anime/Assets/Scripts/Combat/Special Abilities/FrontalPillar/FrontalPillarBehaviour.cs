using RPG.Resource;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class FrontalPillarBehaviour : AbilityBehaviour
    {
        GameObject particleObject;

        public override void Use(GameObject target = null)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100))
            {
                transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
            }

            if (!inUse)
            {
                Debug.Log("should not be here");
                GetComponent<Fighter>().Cancel();
                PlayAbilitySound();
                PlayParticleEffect();
                StartCoroutine("AttackOverTime");
                PlayAbilityAnimation();
                inUse = true;
            }
        }

        protected override void PlayParticleEffect()
        {
            var particlePrefab = config.GetParticlePrefab();
            if (particlePrefab == null) return;
            Vector3 target = transform.position;
            target.y = 1f;
            target.x = .23f;
            target.z = .68f;
            particleObject = Instantiate(
                particlePrefab,
                transform.position,
                transform.rotation, transform
            );
            particleObject.transform.localPosition = target;
            particleObject.GetComponent<FrontalPillarCollider>().config = config;
            particleObject.GetComponent<FrontalPillarCollider>().player = this.gameObject;
            particleObject.GetComponent<FrontalPillarCollider>().attackPerSecond = 1 / (config as FrontalPillarConfig).GetHitsPerSecond();
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
            DisableDamageCollider();
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
            clipOverrides[DEFAULT_LOOP] = (config as FrontalPillarConfig).GetAbilityLoop();
            clipOverrides[DEFAULT_LOOP_END] = (config as FrontalPillarConfig).GetAbilityEnd();
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
                EnableDamageCollider();
                yield return null;
            }
        }

        public void StopLoopAttack()
        {
            StopAllCoroutines();
        }

        private void EnableDamageCollider()
        {
            particleObject.GetComponent<FrontalPillarCollider>().enabled = true;
        }

        private void DisableDamageCollider()
        {
            particleObject.GetComponent<FrontalPillarCollider>().enabled = false;
        }
    }
}

