using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using RPG.Saving;
using RPG.Resource;

namespace RPG.Control
{
    public class AIController : MonoBehaviour, ISaveable
    {
        public bool disableMove;

        [SerializeField] AIActionDatabase aiDatabase;
        [SerializeField] float detectDistance = 5f;
        [SerializeField] float chaseDistance = 10f;
        [SerializeField] float suspicionTime = 5f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = .5f;
        [Range(0,1)]
        [SerializeField] float patrolSpeedFraction = .2f;
        [SerializeField] float wayPointTime = 5f;
        [SerializeField] List<AbilityConfig> abilities = new List<AbilityConfig>();
        AbilityBehaviour[] abilityBehaviours;


        Fighter fighter;
        GameObject player;
        Health health;
        Mover mover;

        Vector3 guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        int currentWaypointIndex = 0;
        float timeSinceLastWaypoint = Mathf.Infinity;
        float currentAggro = 0;
        float actionTimer = 0;

        private void Awake()
        {
            fighter = GetComponent<Fighter>();
            player = GameObject.FindWithTag("Player");
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
        }

        private void Start()
        {
            abilityBehaviours = new AbilityBehaviour[abilities.Count];
            for(int i = 0; i < abilities.Count; i++)
            {
                abilityBehaviours[i] = abilities[i].AttachAbilityTo(this.gameObject, true);
            }
            guardPosition = transform.position;
        }


        public object CaptureState()
        {
            return currentAggro;
        }

        public void RestoreState(object state)
        {
            currentAggro = (float)state;
            currentAggro = 0;
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
            actionTimer += Time.deltaTime;
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
            ActionCondition nextAction = GetMostViableAction();
            if(nextAction != null)
            {
                GetComponent<ActionScheduler>().CancelCurrentAction();
                abilityBehaviours[abilities.IndexOf(nextAction.abilityConfig)].Use();
                disableMove = nextAction.abilityConfig.GetDisableMovement();
                actionTimer = 0;
                fighter.timeSinceLastAttack = 0;
            }
            else
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

        private bool CheckActionConditon(ActionCondition action)
        {
            return (CheckActionDistance(action) && CheckHealthCondition(action) && CheckActionTimer(action));
        }

        private ActionCondition GetMostViableAction()
        {
            if (aiDatabase == null) return null;
            List<ActionCondition> list = new List<ActionCondition>();
            foreach(ActionCondition action in aiDatabase.ActionConditions)
            {
                if (CheckActionConditon(action))
                    list.Add(action);
            }
            return (list.Count <= 0) ? null : list[UnityEngine.Random.Range(0, list.Count)];
        }

        private bool CheckActionDistance(ActionCondition action)
        {
            float distanceFromTarget = Vector3.Distance(player.transform.position, base.transform.position);
            return (distanceFromTarget >= action.minDistance && distanceFromTarget <= action.maxDistance);
        }

        private bool CheckHealthCondition(ActionCondition action)
        {
            Health health = GetComponent<Health>();
            float currentHealthPercent = (health.GetCurrentHealth() / health.GetTotalHealth()) * 100;
            return (currentHealthPercent >= action.minHealthPercent && currentHealthPercent <= action.maxHealthPercent);
        }

        private bool CheckActionTimer(ActionCondition action)
        {
            return actionTimer >= action.actionWaitTimeRange.x;
        }
    }
}

