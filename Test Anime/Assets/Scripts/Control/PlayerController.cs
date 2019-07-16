using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;
using RPG.UI;
using RPG.Resource;
using System;
using RPG.Dialogue;
using UnityEngine.AI;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        enum CursorType
        {
            None,
            Movement,
            Combat,
            Loot,
            Talk
        }

        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }
        [SerializeField] float maxNavMeshProjectionDistance = 1f;
        [SerializeField] float maxNavPathLength = 40f;

        [SerializeField] CursorMapping[] cursorMappings = null;
        bool skill1WasDown;
        bool skill2WasDown;
        bool skill3WasDown;
        bool skill4WasDown;
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
            {
                abilities.CancelLoops();
                return;
            }
            if (InteractWithCombat())
                return;
            if (InteractWithMovement())
                return;
            if (InteractWithItems())
                return;
            if (InteractWithDialogue())
                return;
            SetCursor(CursorType.None);
        }

        private bool InteractWithDialogue()
        {
            if (!canMove)
                return false;
            bool clickInput = Input.GetMouseButtonDown(0);
            RaycastHit[] hits = Physics.RaycastAll((Ray)GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                Voice target = hit.collider.gameObject.GetComponent<Voice>();
                if (target == null)
                    continue;
                SetCursor(CursorType.Talk);
                if (clickInput)
                {
                    if (Vector3.Distance(transform.position, target.transform.position) < 2f)
                    {
                        this.StopAllCoroutines();
                        target.ShowDialog();
                    }
                    else
                    {
                        this.StopAllCoroutines();
                        StartCoroutine(MoveAndTalk(target));
                    }
                }
                return true;
            }
            return false;
        }

        private IEnumerator MoveAndTalk(Voice target)
        {
            mover.MoveTo(target.transform.position, 1f);
            while (Vector3.Distance(transform.position, target.transform.position) > 2f)
            {
                yield return null;
            }
            mover.Cancel();
            yield return new WaitForSeconds(.1f);
            target.ShowDialog();
        }

        private bool InteractWithItems()
        {
            if (!canMove)
                return false;
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
                        this.StopAllCoroutines();
                        Inventory inv = GetComponent<Inventory>();
                        inv.PickUpItem(target.itemInstance);
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
            inv.PickUpItem(target.itemInstance);
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
            bool key4Down = Input.GetKeyDown(KeyCode.Alpha4);
            bool key1Up = Input.GetKeyUp(KeyCode.Alpha1);
            bool key2Up = Input.GetKeyUp(KeyCode.Alpha2);
            bool key3Up = Input.GetKeyUp(KeyCode.Alpha3);
            bool key4Up = Input.GetKeyUp(KeyCode.Alpha4);

            if (key1Up)
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
            if(key4Up)
            {
                abilities.CancelAbility(3);
                skill4WasDown = false;
            }

            if (key1Down)
            {
                if (abilities.GetAbilityAt(0) != null && abilities.HasEnoughEnergy(0))
                {
                    skill1WasDown = true;
                    skill2WasDown = false;
                    skill3WasDown = false;
                    skill4WasDown = false;
                    abilities.CancelLoops();
                    return abilities.AttemptSpecialAbility(0);
                }
                else
                    abilities.PlayOutOfEnergy();
            }
            else if (key2Down)
            {
                if (abilities.GetAbilityAt(1) != null && abilities.HasEnoughEnergy(1))
                {
                    skill2WasDown = true;
                    skill1WasDown = false;
                    skill3WasDown = false;
                    skill4WasDown = false;
                    abilities.CancelLoops();
                    return abilities.AttemptSpecialAbility(1);
                }
                else
                    abilities.PlayOutOfEnergy();
            }
            else if(key3Down)
            {
                if (abilities.GetAbilityAt(2) != null && abilities.HasEnoughEnergy(2))
                {
                    skill3WasDown = true;
                    skill2WasDown = false;
                    skill1WasDown = false;
                    skill4WasDown = false;
                    abilities.CancelLoops();
                    return abilities.AttemptSpecialAbility(2);
                }
                else
                    abilities.PlayOutOfEnergy();
            }
            else if(key4Down)
            {
                if(abilities.GetAbilityAt(3) != null && abilities.HasEnoughEnergy(3))
                {
                    skill4WasDown = true;
                    skill3WasDown = false;
                    skill2WasDown = false;
                    skill1WasDown = false;
                    abilities.CancelLoops();
                    return abilities.AttemptSpecialAbility(3);
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
            else if(skill4WasDown && abilities.OffCooldown(3))
            {
                if (abilities.IsLooping(3))
                {
                    if (abilities.ContinueLoop(3))
                        return false;
                    else
                    {
                        skill4WasDown = false;
                        abilities.PlayOutOfEnergy();
                    }
                }
                else if (abilities.HasEnoughEnergy(3))
                {
                    if (abilities.AbilityInUse()) return InteractWithBasicAttacks();
                    return abilities.AttemptSpecialAbility(3);
                }
                else
                {
                    skill4WasDown = false;
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
                if (Vector3.Distance(transform.position, target.transform.position) < 15)
                    SetCursor(CursorType.Combat);
                if (clickInput && (!targetHealth.IsActive() || targetHealth.target != target.gameObject.GetComponent<Health>()))
                {
                    if (Vector3.Distance(transform.position, target.transform.position) < 15)
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
                return false;

            Vector3 target;
            bool hasHit = RaycastNavMesh(out target);
            if(hasHit)
            {
                if (Input.GetMouseButton(1)/* && !shortestHit.collider.CompareTag("Player") && Vector3.Distance(transform.position, shortestHit.point) > .5f*/)
                {
                    this.StopAllCoroutines();
                    mover.StartMoveAction(target, 1f);
                    SetCursor(CursorType.Movement);
                    return true;
                }
            }

            return false;
        }

        public void ToggleMovement()
        {
            canMove = !canMove;
            if(!canMove)
            {
                mover.Cancel();
            }
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();

            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            //RaycastHit[] hits = Physics.RaycastAll((Ray)GetMouseRay());
            float shortesttYPos = Mathf.Infinity;
            RaycastHit shortestHit = new RaycastHit();
            foreach (RaycastHit hitz in hits)
            {
                if (hitz.collider.gameObject.layer == 8)
                {
                    float hitYPos = hitz.collider.gameObject.transform.position.y;
                    if (shortestHit.collider == null)
                    {
                        shortestHit = hitz;
                        shortesttYPos = hitYPos;
                    }
                    else if (hitYPos > shortesttYPos)
                    {
                        shortestHit = hitz;
                        shortesttYPos = hitYPos;
                    }
                }
            }
            if (shortestHit.collider == null) return false;

            NavMeshHit navMeshHit;
            bool hasCastToNavMesh = NavMesh.SamplePosition(shortestHit.point, out navMeshHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);
            if (!hasCastToNavMesh) return false;

            target = navMeshHit.position;

            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);

            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLength(path) > maxNavPathLength) return false;

            return true;
        }

        private float GetPathLength(NavMeshPath path)
        {
            float total = 0;
            if (path.corners.Length < 2) return total;
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }

            return total;
        }

        public void AllowMovement()
        {
            canMove = true;
        }

        public void DisableMovement()
        {
            canMove = false;
            mover.Cancel();
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
