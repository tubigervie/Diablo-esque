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

        private bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();

            RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition));

            //RaycastHit[] hits = Physics.RaycastAll((Ray)GetMouseRay());
            float shortesttYPos = Mathf.Infinity;
            RaycastHit shortestHit = new RaycastHit();
            foreach (RaycastHit hitz in hits)
            {
                if (hitz.collider.gameObject.layer == 8)
                {
                    float hitYPos = hitz.collider.gameObject.transform.position.y;
                    if (shortestHit.collider == null)
                    {
                        shortestHit = hitz;
                        shortesttYPos = hitYPos;
                    }
                    else if (hitYPos > shortesttYPos)
                    {
                        shortestHit = hitz;
                        shortesttYPos = hitYPos;
                    }
                }
            }
            if (shortestHit.collider == null) return false;
            if (Vector3.Distance(transform.position, shortestHit.point) > (config as BlinkConfig).GetBlinkRange())
            {
                Vector3 direction = (shortestHit.point - transform.position).normalized;
                Vector3 targetPos = transform.position + (direction * (config as BlinkConfig).GetBlinkRange());
                NavMeshHit navMeshHit;
                bool hasCastToNavMesh = NavMesh.SamplePosition(targetPos, out navMeshHit, 1, NavMesh.AllAreas);
                if (!hasCastToNavMesh) return false;
                target = navMeshHit.position;

                NavMeshPath path = new NavMeshPath();
                bool hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);

                if (!hasPath) return false;
                if (path.status != NavMeshPathStatus.PathComplete) return false;
                return true;
            }
            else
            {
                NavMeshHit navMeshHit;
                bool hasCastToNavMesh = NavMesh.SamplePosition(shortestHit.point, out navMeshHit, 1, NavMesh.AllAreas);
                if (!hasCastToNavMesh) return false;
                target = navMeshHit.position;

                NavMeshPath path = new NavMeshPath();
                bool hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);

                if (!hasPath) return false;
                if (path.status != NavMeshPathStatus.PathComplete) return false;
                return true;
            }
           
        }


        public override void Use(GameObject target = null)
        {
            Vector3 targetPos;
            PlayAbilitySound();
            PlayAbilityAnimation();
            PlayParticleEffect();
            if (RaycastNavMesh(out targetPos))
            {
                transform.LookAt(targetPos);
                GetComponent<NavMeshAgent>().Warp(targetPos);
                PlayParticleEffect();
            }
            else
            {
                GetComponent<NavMeshAgent>().Warp(transform.position);
                PlayParticleEffect();
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

