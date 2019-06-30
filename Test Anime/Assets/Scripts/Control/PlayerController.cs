using System.Collections;
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
        bool skill1InputDown;
        bool skill2InputDown;
        bool skill3InputDown;
        //bool skill4InputDown;
        //bool skill5InputDown;

        bool skill1InputUp;
        bool skill2InputUp;
        bool skill3InputUp;
        //bool skill4nputUp;
        //bool skill5InputUp;


        Mover mover;
        Fighter fighter;
        Health health;
        EnemyHealthUI targetHealth;
        SpecialAbilities abilities;
        public bool canMove = true;
        bool firstFrameOnly = false;

        // Start is called before the first frame update
        void Awake()
        {
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            targetHealth = GetComponent<EnemyHealthUI>();
            abilities = GetComponent<SpecialAbilities>();
        }

        private void Start()
        {
            
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
            bool key1Down = Input.GetKeyDown(KeyCode.Alpha1);
            bool key2Down = Input.GetKeyDown(KeyCode.Alpha2);
            bool key3Down = Input.GetKeyDown(KeyCode.Alpha3);
            bool key1Up = Input.GetKeyUp(KeyCode.Alpha1);
            bool key2Up = Input.GetKeyUp(KeyCode.Alpha2);
            bool key3Up = Input.GetKeyUp(KeyCode.Alpha3);

            if (key1Up)
                skill1InputDown = false;
            if (key2Up)
                skill2InputDown = false;
            if (key3Up)
                skill3InputDown = false;

            if (key1Down || skill1InputDown)
            {
                skill1InputDown = true;
                if (abilities.SkipForLooping(0))
                {
                    if (abilities.ContinueLoop(0))
                        return false;
                    else
                    {
                        skill1InputDown = false;
                        abilities.PlayOutOfEnergy();
                    }
                }
                else if (abilities.HasEnoughEnergy(0))
                {
                    return abilities.AttemptSpecialAbility(0);
                }
                else
                {
                    skill1InputDown = false;
                    abilities.PlayOutOfEnergy();
                }
            }
            else if (key2Down || skill2InputDown)
            {
                skill2InputDown = true;
                if (abilities.SkipForLooping(1))
                {
                    if (abilities.ContinueLoop(1))
                        return false;
                    else
                    {
                        skill2InputDown = false;
                        abilities.PlayOutOfEnergy();
                    }
                }
                else if (abilities.HasEnoughEnergy(1))
                {
                    return abilities.AttemptSpecialAbility(1);
                }
                else
                {
                    skill2InputDown = false;
                    abilities.PlayOutOfEnergy();
                }
            }
            else if (key3Down || skill3InputDown)
            {
                skill3InputDown = true;
                if (abilities.SkipForLooping(2))
                {
                    if (abilities.ContinueLoop(2))
                        return false;
                    else
                    {
                        skill3InputDown = false;
                        abilities.PlayOutOfEnergy();
                    }
                }
                else if (abilities.HasEnoughEnergy(2))
                {
                    return abilities.AttemptSpecialAbility(2);
                }
                else
                {
                    skill3InputDown = false;
                    abilities.PlayOutOfEnergy();
                }
            }
            else
            {
                abilities.CancelLoops();
            }

            return InteractWithBasicAttacks();
        }

        private bool InteractWithBasicAttacks()
        {
            bool clickInput = Input.GetMouseButtonDown(0);
            if (!clickInput)
                return false;
            RaycastHit[] hits = Physics.RaycastAll((Ray)GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.collider.gameObject.GetComponent<CombatTarget>();
                if (target == null)
                    continue;
                if (!GetComponent<Fighter>().CanAttack(target.gameObject))
                    continue;
                if (clickInput && (!targetHealth.IsActive() || targetHealth.target != target.gameObject.GetComponent<Health>()))
                {
                    if (Vector3.Distance(transform.position, target.transform.position) < 20)
                        targetHealth.OnEnabled(target.gameObject.GetComponent<Health>());
                }
                else if (clickInput)
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
            if (!canMove)
                return true;
            //RaycastHit hit;
            RaycastHit[] hits = Physics.RaycastAll((Ray)GetMouseRay());
            foreach(RaycastHit hit in hits)
            {
                if(hit.collider.gameObject.layer == 8)
                {
                    if (Input.GetMouseButton(1) && !hit.collider.CompareTag("Player") && Vector3.Distance(transform.position, hit.point) > .5f)
                    {
                        mover.StartMoveAction(hit.point, 1f);
                    }
                    return true;
                }
            }
            return false;
            //bool hasHit = Physics.Raycast((Ray)GetMouseRay(), out hit);
            //if (hasHit)
            //{
            //    if (Input.GetMouseButton(1) && !hit.collider.CompareTag("Player") && Vector3.Distance(transform.position, hit.point) > .5f)
            //    {
            //        mover.StartMoveAction(hit.point, 1f);
            //    }
            //    return true;
            //}
            //return false;
        }

        public void ToggleMovement()
        {
            canMove = !canMove;
            if(!canMove)
            {
                mover.Cancel();
            }
        }
    }
}
