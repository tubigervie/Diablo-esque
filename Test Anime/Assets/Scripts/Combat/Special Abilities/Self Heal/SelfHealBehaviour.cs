using RPG.Control;
using RPG.Resource;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class SelfHealBehaviour : AbilityBehaviour
    {
        PlayerController player = null;

        void Start()
        {
            player = GetComponent<PlayerController>();
        }

        public override void Use(GameObject target)
        {
            PlayAbilitySound();
            GetComponent<Fighter>().Cancel();
            var playerHealth = player.GetComponent<Health>();
            playerHealth.Heal((config as SelfHealConfig).GetExtraHealth());
            PlayParticleEffect();
            PlayAbilityAnimation();
        }
    }
}

