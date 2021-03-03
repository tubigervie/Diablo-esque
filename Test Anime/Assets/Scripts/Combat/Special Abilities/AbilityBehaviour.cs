using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class AnimationClipOverrides : List<KeyValuePair<AnimationClip, AnimationClip>>
    {
        public AnimationClipOverrides(int capacity) : base(capacity) { }

        public AnimationClip this[string name]
        {
            get { return this.Find(x => x.Key.name.Equals(name)).Value; }
            set
            {
                int index = this.FindIndex(x => x.Key.name.Equals(name));
                if (index != -1)
                    this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value);
            }
        }
    }

    public abstract class AbilityBehaviour : MonoBehaviour
    {
        protected AbilityConfig config;

        public const string ABILITY_TRIGGER = "abilityOneShot";
        public const string DEFAULT_ABILITY_ANIMATION = "Cast Spell 01";
        public const string DEFAULT_LOOP_START = "Spin Start";
        public const string DEFAULT_LOOP = "Spin Loop";
        public const string DEFAULT_LOOP_END = "Spin End";
        public bool inUse = false;

        const float PARTICLE_CLEAN_UP_DELAY = 20f;

        public abstract void Use(GameObject target = null);

        public abstract void Cancel();

        public void SetConfig(AbilityConfig configToSet)
        {
            config = configToSet;
        }

        protected abstract void PlayParticleEffect();

        IEnumerator DestroyParticleWhenFinished(GameObject particlePrefab)
        {
            while (particlePrefab.GetComponent<ParticleSystem>().isPlaying)
            {
                yield return new WaitForSeconds(PARTICLE_CLEAN_UP_DELAY);
            }
            Destroy(particlePrefab);
            yield return new WaitForEndOfFrame();
        }

        protected abstract void PlayAbilityAnimation();

        protected void PlayAbilitySound()
        {
            var abilitySound = config.GetRandomAbilitySound();
            if (abilitySound == null) return;
            var audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(abilitySound);
        }

    }

}
