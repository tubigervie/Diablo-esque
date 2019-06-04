﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;
using RPG.UI;
using RPG.Resource;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Mover mover;
        Fighter fighter;
        Health health;
        EnemyHealthUI targetHealth;
        
        // Start is called before the first frame update
        void Start()
        {
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            targetHealth = GetComponent<EnemyHealthUI>();
        }

        // Update is called once per frame
        void Update()
        {
            if (health.IsDead())
                return;
            if (InteractWithCombat())
                return;
            if (InteractWithMovement())
                return;
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll((Ray)GetMouseRay());
            foreach(RaycastHit hit in hits)
            {
                bool clickInput = Input.GetMouseButtonDown(0);
                CombatTarget target = hit.collider.gameObject.GetComponent<CombatTarget>();
                if (target == null)
                    continue;
                if (!GetComponent<Fighter>().CanAttack(target.gameObject))
                    continue;
                if (clickInput && (!targetHealth.IsActive() || targetHealth.target != target.gameObject.GetComponent<Health>()))
                {
                    if(Vector3.Distance(transform.position, target.transform.position) < 20)
                        targetHealth.OnEnabled(target.gameObject.GetComponent<Health>());
                }
                else if(clickInput)
                {
                    fighter.Attack(target.gameObject);
                }
                return true;
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        private bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast((Ray)GetMouseRay(), out hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(0) && Vector3.Distance(transform.position, hit.point) > .5f)
                {
                    mover.StartMoveAction(hit.point, 1f);
                }
                return true;
            }
            return false;
        }
    }
}
