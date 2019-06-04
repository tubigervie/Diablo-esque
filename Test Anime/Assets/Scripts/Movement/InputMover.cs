﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using UnityEngine.AI;
using RPG.Resource;

namespace RPG.Movement
{
    public class InputMover : MonoBehaviour, IAction
    {
        [SerializeField] float maxSpeed = 6f;
        NavMeshAgent agent;
        Animator anim;
        ActionScheduler actionScheduler;
        Health health;
        Vector3 destination;

        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
            actionScheduler = GetComponent<ActionScheduler>();
            health = GetComponent<Health>();
        }

        void Update()
        {
            agent.enabled = !health.IsDead();
            if (!agent.enabled)
                return;
            MoveInput();
        }

        public void MoveInput()
        {
            Vector3 destination = Vector3.zero;

            if (Input.GetKey(KeyCode.W))
                destination += transform.position + (Camera.main.transform.forward);
            else if (Input.GetKey(KeyCode.S))
                destination += transform.position - Camera.main.transform.forward;

            if(destination != Vector3.zero)
            {
                if (Input.GetKey(KeyCode.D))
                    destination += Camera.main.transform.right;
                else if (Input.GetKey(KeyCode.A))
                    destination -= Camera.main.transform.right;
            }
            else
            {
                if (Input.GetKey(KeyCode.D))
                    destination += transform.position + Camera.main.transform.right;
                else if (Input.GetKey(KeyCode.A))
                    destination += transform.position - Camera.main.transform.right;
            }

            if (destination != Vector3.zero)
                StartMoveAction(destination, 1);
        }

        void StartMoveAction(Vector3 destination, float speedFraction)
        {
            actionScheduler.StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            agent.destination = destination;
            agent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            agent.isStopped = false;
        }

        public void Cancel()
        {
            agent.isStopped = true;
        }
    }
}

