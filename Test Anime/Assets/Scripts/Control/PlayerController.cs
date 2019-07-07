using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;
using RPG.UI;
using RPG.Resource;
using System;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        enum CursorType
        {
            None,
            Movement,
            Combat,
            Loot
        }

        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] CursorMapping[] cursorMappings = null;
        bool skill1WasDown;
        bool skill2WasDown;
        bool skill3WasDown;
        //bool skill4InputDown;
        //bool skill5InputDown;

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
            if (InteractWithItems())
                return;
            if (InteractWithMovement())
                return;
            SetCursor(CursorType.None);
        }

        private bool InteractWithItems()
        {
            bool clickInput = Input.GetMouseButtonDown(0);
            RaycastHit[] hits = Physics.RaycastAll((Ray)GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                ItemPickup target = hit.collider.gameObject.GetComponent<ItemPickup>();
                if (target == null)
                    continue;
                SetCursor(CursorType.Loot);
                if (clickInput)
                {
                    if (Vector3.Distance(transform.position, target.transform.position) < 2f)
                    {
                        Inventory inv = GetComponent<Inventory>();
                        inv.AddItemIntoInventory(target.itemInstance);
                        if (target.wasFromInventory)
                        {
                            inv.RemoveDroppedItem(target);
                        }
                        Destroy(target.gameObject);
                    }
                    else
                    {
                        this.StopAllCoroutines();
                        StartCoroutine(MoveAndCollect(target));
                    }
                }
                return true;
            }
            return false;
        }

        private IEnumerator MoveAndCollect(ItemPickup target)
        {
            //Vector3 directionOfTravel = (transform.position - target.transform.position).normalized;
            //Vector3 targetPosition = target.transform.position + (directionOfTravel * 3); 
            mover.MoveTo(target.transform.position, 1f);
            while(Vector3.Distance(transform.position, target.transform.position) > 2f)
            {
                yield return null;
            }
            mover.Cancel();
            yield return new WaitForSeconds(.1f);
            Inventory inv = GetComponent<Inventory>();
            inv.AddItemIntoInventory(target.itemInstance);
            if (target.wasFromInventory)
            {
                inv.RemoveDroppedItem(target);
            }
            Destroy(target.gameObject);
        }

        private void OnDisable()
        {
            SetCursor(CursorType.None);
        }

        private bool InteractWithCombat()
        {
            bool key1Down = Input.GetKeyDown(KeyCode.Alpha1);
            bool key2Down = Input.GetKeyDown(KeyCode.Alpha2);
            bool key3Down = Input.GetKeyDown(KeyCode.Alpha3);
            bool key1Up = Input.GetKeyUp(KeyCode.Alpha1);
            bool key2Up = Input.GetKeyUp(KeyCode.Alpha2);
            bool key3Up = Input.GetKeyUp(KeyCode.Alpha3);

            if(key1Up)
            {
                abilities.CancelAbility(0);
                skill1WasDown = false;
            }
            if(key2Up)
            {
                abilities.CancelAbility(1);
                skill2WasDown = false;
            }
            if(key3Up)
            {
                abilities.CancelAbility(2);
                skill3WasDown = false;
            }

            if (key1Down)
            {
                if (abilities.HasEnoughEnergy(0))
                {
                    skill1WasDown = true;
                    skill2WasDown = false;
                    skill3WasDown = false;
                    abilities.CancelLoops();
                    return abilities.AttemptSpecialAbility(0);
                }
                else
                    abilities.PlayOutOfEnergy();
            }
            else if (key2Down)
            {
                if (abilities.HasEnoughEnergy(1))
                {
                    skill2WasDown = true;
                    skill1WasDown = false;
                    skill3WasDown = false;
                    abilities.CancelLoops();
                    return abilities.AttemptSpecialAbility(1);
                }
                else
                    abilities.PlayOutOfEnergy();
            }
            else if(key3Down)
            {
                if (abilities.HasEnoughEnergy(2))
                {
                    skill3WasDown = true;
                    skill2WasDown = false;
                    skill1WasDown = false;
                    abilities.CancelLoops();
                    return abilities.AttemptSpecialAbility(2);
                }
                else
                    abilities.PlayOutOfEnergy();
            }
            else if(skill1WasDown && abilities.OffCooldown(0))
            {
                if (abilities.IsLooping(0))
                {
                    if (abilities.ContinueLoop(0))
                        return false;
                    else
                    {
                        skill1WasDown = false;
                        abilities.PlayOutOfEnergy();
                    }
                }
                else if (abilities.HasEnoughEnergy(0))
                {
                    if (abilities.AbilityInUse()) return InteractWithBasicAttacks();
                    return abilities.AttemptSpecialAbility(0);
                }
                else
                {
                    skill1WasDown = false;
                    if (abilities.AbilityInUse()) return InteractWithBasicAttacks();
                    abilities.PlayOutOfEnergy();
                }
            }
           else if (skill2WasDown && abilities.OffCooldown(1))
            {
                if (abilities.IsLooping(1))
                {
                    if (abilities.ContinueLoop(1))
                        return false;
                    else
                    {
                        skill2WasDown = false;
                        abilities.PlayOutOfEnergy();
                    }
                }
                else if (abilities.HasEnoughEnergy(1))
                {
                    if (abilities.AbilityInUse()) return InteractWithBasicAttacks();
                    return abilities.AttemptSpecialAbility(1);
                }
                else
                {
                    skill2WasDown = false;
                    if (abilities.AbilityInUse()) return InteractWithBasicAttacks();
                    abilities.PlayOutOfEnergy();
                }
            }
            else if (skill3WasDown && abilities.OffCooldown(2))
            {
                if (abilities.IsLooping(2))
                {
                    if (abilities.ContinueLoop(2))
                        return false;
                    else
                    {
                        skill3WasDown = false;
                        abilities.PlayOutOfEnergy();
                    }
                }
                else if (abilities.HasEnoughEnergy(2))
                {
                    if (abilities.AbilityInUse()) return InteractWithBasicAttacks();
                    return abilities.AttemptSpecialAbility(2);
                }
                else
                {
                    skill3WasDown = false;
                    if (abilities.AbilityInUse()) return InteractWithBasicAttacks();
                    abilities.PlayOutOfEnergy();
                }
            }
            else
            {
                //abilities.CancelLoops();
            }

            return InteractWithBasicAttacks();
        }

        private bool InteractWithBasicAttacks()
        {
            bool clickInput = Input.GetMouseButtonDown(0);
            RaycastHit[] hits = Physics.RaycastAll((Ray)GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.collider.gameObject.GetComponent<CombatTarget>();
                if (target == null)
                    continue;
                if (!GetComponent<Fighter>().CanAttack(target.gameObject))
                    continue;
                SetCursor(CursorType.Combat);
                if (clickInput && (!targetHealth.IsActive() || targetHealth.target != target.gameObject.GetComponent<Health>()))
                {
                    if (Vector3.Distance(transform.position, target.transform.position) < 20)
                        targetHealth.OnEnabled(target.gameObject.GetComponent<Health>(), target.displayID);
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
            float shortesttYPos = Mathf.Infinity;
            RaycastHit shortestHit = new RaycastHit();
            foreach(RaycastHit hit in hits)
            {
                if(hit.collider.gameObject.layer == 8)
                {
                    float hitYPos = hit.collider.gameObject.transform.position.y;
                    if (shortestHit.collider == null)
                    {
                        shortestHit = hit;
                        shortesttYPos = hitYPos;
                    }
                    else if(hitYPos > shortesttYPos)
                    {
                        shortestHit = hit;
                        shortesttYPos = hitYPos;
                    }
                }
            }
            if (shortestHit.collider == null) return false;
            if (Input.GetMouseButton(1) && !shortestHit.collider.CompareTag("Player") && Vector3.Distance(transform.position, shortestHit.point) > .5f)
            {
                this.StopAllCoroutines();
                mover.StartMoveAction(shortestHit.point, 1f);
                SetCursor(CursorType.Movement);
                return true;
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

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping mapping in cursorMappings)
            {
                if (mapping.type == type)
                {
                    return mapping;
                }
            }
            return cursorMappings[0];
        }
    }
}
