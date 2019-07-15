using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Combat
{
    public class BlinkBehaviour : AbilityBehaviour
    {
        public override void Cancel()
        {
            return;
        }

        public override void Use(GameObject target = null)
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
            if(hasHit)
            {
                if(Vector3.Distance(transform.position, hit.point) > (config as BlinkConfig).GetBlinkRange())
                {
                    Debug.Log("in here");
                    Vector3 direction = (hit.point - transform.position).normalized;
                    Vector3 targetPos = transform.position + (direction * (config as BlinkConfig).GetBlinkRange());
                    NavMeshHit navHit;
                    bool hasSample = NavMesh.SamplePosition(targetPos, out navHit, 1f, NavMesh.AllAreas);
                    PlayAbilitySound();
                    PlayAbilityAnimation();
                    PlayParticleEffect();
                    transform.LookAt(targetPos);

                    if (!hasSample)
                    {
                        GetComponent<NavMeshAgent>().Warp(transform.position);
                        PlayParticleEffect();
                    }
                    else
                    {
                        GetComponent<NavMeshAgent>().Warp(navHit.position);
                        PlayParticleEffect();
                    }
                }
                else
                {
                    PlayAbilitySound();
                    PlayAbilityAnimation();
                    PlayParticleEffect();

                    NavMeshHit navHit;
                    bool hasSample = NavMesh.SamplePosition(hit.point, out navHit, 1f, NavMesh.AllAreas);

                    transform.LookAt(hit.point);

                    if (!hasSample)
                    {
                        GetComponent<NavMeshAgent>().Warp(transform.position);
                        PlayParticleEffect();
                    }
                    else
                    {
                        GetComponent<NavMeshAgent>().Warp(navHit.position);
                        PlayParticleEffect();
                    }
                }

            }
            return;
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

        protected override void PlayParticleEffect()
        {
            GameObject particle = Instantiate(config.GetParticlePrefab(), transform);
            particle.transform.parent = null;
            Destroy(particle, 3);
            return;
        }

    }
}

