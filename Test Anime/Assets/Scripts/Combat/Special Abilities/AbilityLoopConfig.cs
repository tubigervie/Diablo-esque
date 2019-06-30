using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class AbilityLoopConfig : AbilityConfig
    {
        [Header("Area Effect Specific")]
        [SerializeField] AnimationClip abilityLoopAnimation;
        [SerializeField] AnimationClip abilityEndAnimation;

        public override AbilityBehaviour GetBehaviourComponent(GameObject objectToAttachTo)
        {
            return objectToAttachTo.AddComponent<AreaEffectBehaviour>();
        }

    }
}
    

