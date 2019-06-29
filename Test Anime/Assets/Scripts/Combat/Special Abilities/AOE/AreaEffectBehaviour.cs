using RPG.Control;
using RPG.Resource;
using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class AreaEffectBehaviour : AbilityBehaviour
    {

        public override void Use(GameObject target)
        {
            GetComponent<Fighter>().Cancel();
            PlayAbilitySound();
            DealRadialDamage();
            PlayParticleEffect();
            PlayAbilityAnimation();
        }

        private void DealRadialDamage()
        {
            // Static sphere cast for targets
            RaycastHit[] hits = Physics.SphereCastAll(
                transform.position,
                (config as AreaEffectConfig).GetRadius(),
                Vector3.up,
                (config as AreaEffectConfig).GetRadius()
            );

            foreach (RaycastHit hit in hits)
            {
                var damageable = hit.collider.gameObject.GetComponent<Health>();
                bool hitPlayer = hit.collider.gameObject.GetComponent<PlayerController>();
                if (damageable != null && !hitPlayer)
                {
                    float damageToDeal = (config as AreaEffectConfig).GetDamageToEachTarget(GetComponent<Fighter>().GetBaseDamage()) + GetComponent<BaseStats>().GetBaseStat(Stat.Damage); //replace with just GetStat once weapons stats are in
                    damageable.TakeDamage(this.gameObject, damageToDeal);
                }
            }
        }
    }
}

