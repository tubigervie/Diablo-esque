using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Health target = null;
    [SerializeField] float speed = 1;
    [SerializeField] bool isHoming = true;
    [SerializeField] GameObject hitEffect = null;
    float damage = 0;

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

    public void SetTarget(Health target, float damage)
    {
        this.target = target;
        this.damage = damage;
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
            target.TakeDamage(damage);
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
