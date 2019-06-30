using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Resource;

public class Projectile : MonoBehaviour
{
    Health target = null;
    [SerializeField] float speed = 1;
    [SerializeField] bool isHoming = true;
    [SerializeField] GameObject hitEffect = null;
    [SerializeField] AudioClip hitSound;
    float damage = 0;
    bool isCritical;
    GameObject instigator;

    private void Start()
    {
        transform.LookAt(GetAimLocation());
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;
        if(isHoming)
        {
            if(!target.IsDead())
                transform.LookAt(GetAimLocation());
        }
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    public void SetTarget(Health target, GameObject instigator, float damage, bool isCritical)
    {
        this.instigator = instigator;
        this.target = target;
        this.damage = damage;
        this.isCritical = isCritical;
    }

    private Vector3 GetAimLocation()
    {
        CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
        if(targetCapsule == null)
        {
            return target.transform.position;
        }
        return target.transform.position + Vector3.up * targetCapsule.height / 1.3f;
    }

    private void OnTriggerEnter(Collider other)
    {
        Health otherHealth = other.GetComponent<Health>();
        if (otherHealth != null && otherHealth == target)
        {
            if (target.IsDead())
                return;
            if (hitSound != null)
                other.GetComponent<AudioSource>().PlayOneShot(hitSound);
            target.TakeDamage(instigator, damage, isCritical);
            TrailRenderer trail = GetComponentInChildren<TrailRenderer>();
            if(trail != null)
            {
                trail.gameObject.transform.parent = null;
                Destroy(trail.gameObject, 1f);
            }
            GameObject fx;
            if (hitEffect != null)
            {
                fx = Instantiate(hitEffect, GetAimLocation(), transform.rotation);
                Destroy(fx, 1f);
            }       
            Destroy(this.gameObject);
        }
    }
}
