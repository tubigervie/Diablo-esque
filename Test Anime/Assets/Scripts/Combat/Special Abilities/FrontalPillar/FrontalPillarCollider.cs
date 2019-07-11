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


    List<Dictionary<Collider, float>> colliderTimer = new List<Dictionary<Collider, float>>();

    private void OnTriggerStay(Collider other)
    {
        var damageable = other.gameObject.GetComponent<Health>();
        bool hitPlayer = other.gameObject.GetComponent<PlayerController>();
        if (damageable != null && !hitPlayer)
        {
            bool foundinList = false;
            foreach(Dictionary<Collider, float> colliderPair in colliderTimer)
            {
                if(colliderPair.ContainsKey(other))
                {
                    foundinList = true;
                    if(colliderPair[other] <= 0)
                    {
                        float damageToDeal = (config as FrontalPillarConfig).GetDamageToEachTarget(player.GetComponent<Fighter>().GetDamage()) - other.gameObject.GetComponent<BaseStats>().GetDefense(); //replace with just GetStat once weapons stats are in
                        bool shouldCrit = player.GetComponent<Fighter>().ShouldCrit();
                        if (shouldCrit)
                            damageToDeal *= 1.5f;
                        damageable.TakeDamage(this.gameObject, damageToDeal, shouldCrit);
                        colliderPair[other] = attackPerSecond;
                    }
                    break;
                }
            }
            if(!foundinList)
            {
                Dictionary<Collider, float> newColliderPair = new Dictionary<Collider, float>();
                newColliderPair[other] = 0;
                float damageToDeal = (config as FrontalPillarConfig).GetDamageToEachTarget(player.GetComponent<Fighter>().GetDamage()) - other.gameObject.GetComponent<BaseStats>().GetDefense(); //replace with just GetStat once weapons stats are in
                bool shouldCrit = player.GetComponent<Fighter>().ShouldCrit();
                if (shouldCrit)
                    damageToDeal *= 1.5f;
                damageable.TakeDamage(this.gameObject, damageToDeal, shouldCrit);
                newColliderPair[other] = attackPerSecond;
                colliderTimer.Add(newColliderPair);
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (Dictionary<Collider, float> colliderPair in colliderTimer)
        {
            if (colliderPair.ContainsKey(other))
            {
                colliderTimer.Remove(colliderPair);
                break;
            }
        }
    }

    private void FixedUpdate()
    {
        List<Dictionary<Collider, float>> currentList = colliderTimer;
        for(int i = 0; i < currentList.Count; i++)
        {
            List<Collider> colliders = new List<Collider>();
            foreach(Collider collider in currentList[i].Keys)
            {
                colliders.Add(collider);
            }
            for(int x = 0; x < colliders.Count; i++)
            {
                currentList[i][colliders[x]] -= Time.fixedDeltaTime;
            }
        }
    }
}
