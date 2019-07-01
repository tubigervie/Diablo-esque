using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using UnityEngine.AI;
using RPG.Resource;
using RPG.Movement;

namespace RPG.Control
{
    public class InputMover : MonoBehaviour, IAction
    {
        [SerializeField] float maxSpeed = 6f;
        NavMeshAgent agent;
        Animator anim;
        ActionScheduler actionScheduler;
        Health health;
        Vector3 destination;
        bool canMove = true;

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
            if (!agent.enabled || !canMove)
                return;
            MoveInput();
        }

        public void ToggleMovement()
        {
            canMove = !canMove;
        }

        public void MoveInput()
        {
            Vector3 destination = Vector3.zero;
            float inputX = Input.GetAxisRaw("Horizontal");
            float inputY = Input.GetAxisRaw("Vertical");

            if (inputY > 0)
                destination += transform.position + (Camera.main.transform.forward);
            else if (inputY < 0)
                destination += transform.position - Camera.main.transform.forward;

            if(destination != Vector3.zero)
            {
                if (inputX > 0)
                    destination += Camera.main.transform.right;
                else if (inputX < 0)
                    destination -= Camera.main.transform.right;
            }
            else
            {
                if (inputX > 0)
                    destination += transform.position + Camera.main.transform.right;
                else if (inputX < 0)
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

