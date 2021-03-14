using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;
using RPG.Resource;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] GameObject waypointMarker;
        [SerializeField] float maxSpeed = 6f;

        Transform target;
        NavMeshAgent agent;
        Ray lastRay;
        Animator anim;
        ActionScheduler actionScheduler;
        Health health;

        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
            actionScheduler = GetComponent<ActionScheduler>();
            health = GetComponent<Health>();
        }

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            agent.enabled = !health.IsDead();
            UpdateAnimator();
            if (waypointMarker != null && waypointMarker.activeInHierarchy && !agent.hasPath)
                waypointMarker.SetActive(false);
        }

        void UpdateAnimator()
        {
            Vector3 velocity = agent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            anim.SetFloat("forwardSpeed", speed);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            agent.destination = destination;         
            agent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            agent.isStopped = false;
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            actionScheduler.StartAction(this);
            MoveTo(destination, speedFraction);
            if(waypointMarker != null)
            {
                waypointMarker.SetActive(true);
                Vector3 target = destination;
                target.y += .2f;
                waypointMarker.transform.position = target;
            }
        }

        public void Cancel()
        {
            if(agent.isActiveAndEnabled)
                agent.isStopped = true;
            if(waypointMarker != null)
                waypointMarker.SetActive(false);
        }

        public void DisableMarker()
        {
            if (waypointMarker != null)
                waypointMarker.SetActive(false);
        }

        public object CaptureState()
        {
            SerializableVector3 pos = new SerializableVector3(transform.position);
            SerializableVector3 rot = new SerializableVector3(transform.eulerAngles);
            Tuple<SerializableVector3, SerializableVector3> serialized = new Tuple<SerializableVector3, SerializableVector3>(pos, rot);
            return serialized;
        }

        public void RestoreState(object state)
        {
            Tuple<SerializableVector3, SerializableVector3> stateInfo = (Tuple<SerializableVector3, SerializableVector3>) state;
            SerializableVector3 position = stateInfo.Item1;
            SerializableVector3 rotation = stateInfo.Item2;
            agent.enabled = false;
            transform.position = position.ToVector();
            transform.eulerAngles = rotation.ToVector();
            agent.enabled = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}

