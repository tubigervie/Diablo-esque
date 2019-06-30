using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Combat
{
    [CreateAssetMenu(menuName = ("RPG/Special Abiltiy/Spin Attack"))]
    public class SpinAttackConfig : AbilityConfig
    {
        [Header("Loop Specific")]
        [SerializeField] AnimationClip abilityLoopAnimation;
        [SerializeField] AnimationClip abilityEndAnimation;

        [Header("Spin Attack Specific")]
        [SerializeField] float radius = 5f;
        [SerializeField] float percentageDamage;
        [SerializeField] float hitsPerSecond = 2;
        [SerializeField] AudioClip attackSoundEffect;

        public override AbilityBehaviour GetBehaviourComponent(GameObject objectToAttachTo)
        {
            return objectToAttachTo.AddComponent<SpinAttackBehaviour>();
        }

        public float GetDamageToEachTarget(float damage)
        {
            return damage * (percentageDamage / 100);
        }

        public float GetRadius()
        {
            return radius;
        }

        public float GetHitsPerSecond()
        {
            return hitsPerSecond;
        }

        public AnimationClip GetAbilityLoop()
        {
            return abilityLoopAnimation;
        }

        public AnimationClip GetAbilityEnd()
        {
            return abilityEndAnimation;
        }

        public AudioClip GetAttackClip()
        {
            return attackSoundEffect;
        }
    }
}

