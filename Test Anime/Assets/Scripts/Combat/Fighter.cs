using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        ActionScheduler actionScheduler;
        Transform combatTarget;
        Mover mover;
        [SerializeField] float weaponRange = 2f;

        private void Start()
        {
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
        }

        private void Update()
        {
            if (combatTarget == null)
                return;

            if(combatTarget != null && !GetIsInRange())
            {
                mover.MoveTo(combatTarget.position);
            }
            else
            {
                mover.Cancel();
                Debug.Log("Has hit");
            }
        }

        public void Attack(CombatTarget target)
        {
            actionScheduler.StartAction(this);
            combatTarget = target.transform; 
        }

        public void Cancel()
        {
            combatTarget = null;
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, combatTarget.position) < weaponRange;
        }
    }
}
