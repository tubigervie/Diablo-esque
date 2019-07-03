using RPG.Combat;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.CameraUI
{
    public class ItemTooltipSpawner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] ItemTooltip tooltipPrefab;

        ItemTooltip _tooltip;

        private void OnDestroy()
        {
            ClearTooltip();
        }

        private void OnDisable()
        {
            ClearTooltip();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("should be reading tooltip");
            var item = GetComponent<IItemHolder>().item;
            if (!item) return;

            var parentCanvas = GetComponentInParent<Canvas>();

            if (!_tooltip)
            {
                _tooltip = Instantiate(tooltipPrefab, parentCanvas.transform);
            }

            _tooltip.title = item.displayName;
            Debug.Log("item display " + item.displayName);
            Debug.Log("tooltip " + _tooltip.title);
            _tooltip.body = item.description;

            Weapon weap = item as Weapon;

            _tooltip.value = "Attack: " + weap.GetDamageRange().min + " - " + weap.GetDamageRange().max;

            foreach(StatModifier modifier in item.statModifiers)
            {
                _tooltip.SpawnModifier(modifier.bonusType, modifier.amount, modifier.stat);
            }

            PositionTooltip();
        }

        private void PositionTooltip()
        {
            // Required to ensure corners are updated by positioning elements.
            Canvas.ForceUpdateCanvases();

            var tooltipCorners = new Vector3[4];
            _tooltip.GetComponent<RectTransform>().GetWorldCorners(tooltipCorners);
            var slotCorners = new Vector3[4];
            GetComponent<RectTransform>().GetWorldCorners(slotCorners);

            bool below = transform.position.y > Screen.height / 2;
            bool right = transform.position.x < Screen.width / 2;

            int slotCorner = GetCornerIndex(below, right);
            int tooltipCorner = GetCornerIndex(!below, !right);

            _tooltip.transform.position = slotCorners[slotCorner] - tooltipCorners[tooltipCorner] + _tooltip.transform.position;
        }

        private int GetCornerIndex(bool below, bool right)
        {
            if (below && !right) return 0;
            else if (!below && !right) return 1;
            else if (!below && right) return 2;
            else return 3;

        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ClearTooltip();
        }

        private void ClearTooltip()
        {
            if (_tooltip)
            {
                Destroy(_tooltip.gameObject);
            }
        }
    }
}