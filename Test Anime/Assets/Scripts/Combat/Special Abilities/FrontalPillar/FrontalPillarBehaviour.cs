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
            //RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit[] hits = Physics.RaycastAll(ray, 100);
            float shortesttYPos = Mathf.Infinity;
            RaycastHit shortestHit = new RaycastHit();
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.layer == 8)
                {
                    float hitYPos = hit.collider.gameObject.transform.position.y;
                    if (shortestHit.collider == null)
                    {
                        shortestHit = hit;
                        shortesttYPos = hitYPos;
                    }
                    else if (hitYPos > shortesttYPos)
                    {
                        shortestHit = hit;
                        shortesttYPos = hitYPos;
                    }
                }
            }
            if (shortestHit.collider != null)
            {
                transform.LookAt(new Vector3(shortestHit.point.x, transform.position.y, shortestHit.point.z));
            }
            //if (Physics.Raycast(ray, out hit, 100))
            //{
            //    transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
            //}

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
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.loop = false;
            audioSource.clip = null;
            GetComponent<Animator>().SetBool("abilityLoop", false);
            //GetComponent<Fighter>().timeSinceLastAttack = 0;
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
            audioSource.clip = (config as FrontalPillarConfig).GetAttackClip();
            audioSource.Play();
            audioSource.loop = true;
            while (true)
            {
                EnableDamageCollider();
                yield return new WaitForSeconds(1f);
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

