using RPG.Combat;
using RPG.Control;
using RPG.Resource;
using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontalPillarCollider : MonoBehaviour
{
    public AbilityConfig config;
    public GameObject player;
    public float attackPerSecond;


    List<CustomKeyValuePair<Collider, float>> colliderTimer = new List<CustomKeyValuePair<Collider, float>>();

    private void OnTriggerStay(Collider other)
    {
        var damageable = other.gameObject.GetComponent<Health>();
        bool hitPlayer = other.gameObject.GetComponent<PlayerController>();
        if (damageable != null && !hitPlayer)
        {
            bool foundinList = false;
            foreach(CustomKeyValuePair<Collider, float> colliderPair in colliderTimer)
            {
                if(colliderPair.key == other)
                {
                    foundinList = true;
                    if (colliderPair.value <= 0)
                    {
                        float damageToDeal = (config as FrontalPillarConfig).GetDamageToEachTarget(player.GetComponent<Fighter>().GetDamage()) - colliderPair.key.gameObject.GetComponent<BaseStats>().GetDefense(); //replace with just GetStat once weapons stats are in
                        bool shouldCrit = player.GetComponent<Fighter>().ShouldCrit();
                        if (shouldCrit)
                            damageToDeal *= 1.5f;
                        damageable.TakeDamage(player, damageToDeal, shouldCrit);
                        colliderPair.value = attackPerSecond;
                    }
                    break;
                }
            }
            if(!foundinList)
            {
                CustomKeyValuePair<Collider, float> newColliderPair = new CustomKeyValuePair<Collider, float>(other, 0);
                float damageToDeal = (config as FrontalPillarConfig).GetDamageToEachTarget(player.GetComponent<Fighter>().GetDamage()) - other.gameObject.GetComponent<BaseStats>().GetDefense(); //replace with just GetStat once weapons stats are in
                bool shouldCrit = player.GetComponent<Fighter>().ShouldCrit();
                if (shouldCrit)
                    damageToDeal *= 1.5f;
                damageable.TakeDamage(player, damageToDeal, shouldCrit);
                newColliderPair.value = attackPerSecond;
                colliderTimer.Add(newColliderPair);
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (CustomKeyValuePair<Collider, float> colliderPair in colliderTimer)
        {
            if (other == colliderPair.key)
            {
                colliderTimer.Remove(colliderPair);
                break;
            }
        }
    }

    private void Update()
    {
        List<CustomKeyValuePair<Collider, float>> current = new List<CustomKeyValuePair<Collider, float>>();
        for(int i = 0; i < colliderTimer.Count; i++)
        {
            if(colliderTimer[i].key != null)
                current.Add(colliderTimer[i]);
        }
        if (current.Count < 1) return;
        foreach (CustomKeyValuePair<Collider, float> colliderPairs in current)
        {

            colliderPairs.value -= Time.deltaTime;
            
        }
    }
}
