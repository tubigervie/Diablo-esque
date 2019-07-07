using RPG.Combat;
using System;
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
            var item = GetComponent<IItemHolder>().item;
            if (item == null) return;
            if (item.itemBase == null) return;
            var parentCanvas = GetComponentInParent<Canvas>();

            if (!_tooltip)
            {
                _tooltip = Instantiate(tooltipPrefab, parentCanvas.transform);
            }

            _tooltip.title = item.itemBase.displayName;
            //Debug.Log("item display " + item.itemBase.displayName);
            //Debug.Log("tooltip " + _tooltip.title);
            _tooltip.body = item.itemBase.description;
            if (item.itemBase.isWeapon)
            {
                WeaponInstance weap = new WeaponInstance(item.itemBase as Weapon, item.properties);
                _tooltip.value = "DMG: " + (float)Math.Round(weap.GetDamageRange().min, 1) + " - " + (float)Math.Round(weap.GetDamageRange().max, 1);
            }
            else
                _tooltip.value = "";
            _tooltip.setRarity(item.properties.rarity);

            foreach(StatModifier modifier in item.properties.statModifiers)
            {
                _tooltip.SpawnModifier(modifier.bonusType, modifier.maxAmount, modifier.stat);
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