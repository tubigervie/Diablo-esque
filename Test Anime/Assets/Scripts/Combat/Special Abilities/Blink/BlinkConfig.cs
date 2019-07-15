using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(menuName = ("RPG/Special Abiltiy/Blink"))]
    public class BlinkConfig : AbilityConfig
    {
        [Header("Blink Specific")]
        [SerializeField] float range = 10f;

        public override AbilityBehaviour GetBehaviourComponent(GameObject objectToAttachTo)
        {
            return objectToAttachTo.AddComponent<BlinkBehaviour>();
        }

        public float GetBlinkRange()
        {
            return range;
        }
    }
}

