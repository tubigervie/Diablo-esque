using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public abstract class AbilityConfig : ScriptableObject
    {
        [Header("Spcial Ability General")]
        [SerializeField] float energyCost = 10f;
        [SerializeField] float coolDownTime = 3f;
        [SerializeField] GameObject particlePrefab;
        [SerializeField] AnimationClip abilityAnimation;
        [SerializeField] AudioClip[] audioClips;
        [SerializeField] float clipSpeed = 1;
        [SerializeField] bool disableMovement = true;
        [SerializeField] bool isLooping = false;
        [SerializeField] Sprite skillIcon;
        [HideInInspector] public AbilityBehaviour behaviour;

        public abstract AbilityBehaviour GetBehaviourComponent(GameObject objectToAttachTo);

        public void AttachAbilityTo(GameObject objectToattachTo)
        {
            AbilityBehaviour behaviourComponent = GetBehaviourComponent(objectToattachTo);
            behaviourComponent.SetConfig(this);
            behaviour = behaviourComponent;
        }

        public void Use(GameObject target)
        {
            behaviour.Use(target);
        }

        public float GetEnergyCost()
        {
            return energyCost;
        }

        public GameObject GetParticlePrefab()
        {
            return particlePrefab;
        }

        public AnimationClip GetAbilityAnimation()
        {
            return abilityAnimation;
        }

        public Sprite GetIcon()
        {
            return skillIcon;
        }

        public float GetCooldownTime()
        {
            return coolDownTime;
        }

        public AudioClip GetRandomAbilitySound()
        {
            if (audioClips.Length == 0)
                return null;
            return audioClips[Random.Range(0, audioClips.Length)];
        }

        public float GetAnimationSpeed()
        {
            return clipSpeed;
        }

        public bool GetDisableMovement()
        {
            return disableMovement;
        }

        public bool IsLooping()
        {
            return isLooping;
        }
    }
}

