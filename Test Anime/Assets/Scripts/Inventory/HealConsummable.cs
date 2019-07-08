using RPG.Resource;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = ("RPG/Inventory/Heal Consummable"))]
public class HealConsummable : ConsumableItem
{
    [SerializeField] float amount;
    [SerializeField] GameObject particle;
    [SerializeField] AudioClip audio;

    public override void Use(GameObject entity)
    {
        Health health = entity.GetComponent<Health>();
        health.Heal(amount);
        Instantiate(particle, entity.transform);
        entity.GetComponent<AudioSource>().PlayOneShot(audio);
    }
}
