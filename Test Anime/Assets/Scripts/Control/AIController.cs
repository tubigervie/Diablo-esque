using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using RPG.Saving;
using RPG.Resources;

namespace RPG.Control
{
    public class AIController : MonoBehaviour, ISaveable
    {
        [SerializeField] float detectDistance = 5f;
        [SerializeField] float chaseDistance = 10f;
        [SerializeField] float suspicionTime = 5f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = .5f;
        [Range(0,1)]
        [SerializeField] float patrolSpeedFraction = .2f;
        [SerializeField] float wayPointTime = 5f;

        Fighter fighter;
        GameObject player;
        Health health;
        Mover mover;

        Vector3 guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        int currentWaypointIndex = 0;
        float timeSinceLastWaypoint = Mathf.Infinity;
        [SerializeField] float currentAggro = 0;

        private void Start()
        {
            fighter = GetComponent<Fighter>();
            player = GameObject.FindWithTag("Player");
            health = GetComponent<Health>();
            guardPosition = transform.position;
            mover = GetComponent<Mover>();
        }


        public object CaptureState()
        {
            return currentAggro;
        }

        public void RestoreState(object state)
        {
            currentAggro = (float)state;
        }


        private void Update()
        {
            if (health.IsDead())
                return;
            if ((InAttackRange() || (Vector3.Distance(player.transform.position, transform.position) < chaseDistance && currentAggro > 0)) && fighter.CanAttack(player))
            {
                timeSinceLastSawPlayer = 0;
                AttackBehavior();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehavior();
            }
            else
            {
                PatrolBehavior();
            }

            UpdateTimers();
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceLastWaypoint += Time.deltaTime;
            currentAggro -= Time.deltaTime;
            if (currentAggro < 0)
                currentAggro = 0;
        }

        private void PatrolBehavior()
        {
            Vector3 nextPosition = guardPosition;
            if(patrolPath != null)
            {
                if(AtWaypoint())
                {
                    CycleWaypoint();
                    timeSinceLastWaypoint = 0;
                }
                nextPosition = GetCurrentWaypoint();
            }
            if(timeSinceLastWaypoint > wayPointTime)
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private void SuspicionBehavior()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehavior()
        {
            fighter.Attack(player);
        }

        private bool InAttackRange()
        {
            return Vector3.Distance(player.transform.position, transform.position) < detectDistance;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, detectDistance);
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }

        public void AddAggro(float damage)
        {
            currentAggro += Mathf.Clamp(damage, 0, 10);
            if (currentAggro > 100)
                currentAggro = 100;
        }
    }
}

