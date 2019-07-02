using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipSlotUI : MonoBehaviour, IItemHolder, IDropHandler
{
    [SerializeField] Image _iconImage;
    [SerializeField] EquippableItem.EquipLocation _type;
    public int index { get; set; }

    Inventory _inventory;
    InventoryItem _item;

    public Inventory inventory { set { _inventory = value; } }

    public EquippableItem.EquipLocation type { get => _type; }

    public InventoryItem item
    {
        get => _item;
        set
        {
            SetItem(value);
        }
    }

    public void SetItem(InventoryItem item)
    {
        _item = item;

        if (item == null)
        {
            _iconImage.enabled = false;
        }
        else
        {
            _iconImage.enabled = true;
            _iconImage.sprite = item.icon;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        var invItem = eventData.pointerDrag.GetComponent<ItemInventoryUI>();
        var equipItem = eventData.pointerDrag.GetComponent<ItemEquippedUI>();
        if (invItem == null && equipItem != null)
        {
            Debug.Log("Dropped from equipped into equipped");
            if (equipItem.parentSlot.index == index) return;
        }
        else
        {
            Debug.Log("Dropped from inv into equip slot");
            if (invItem.parentSlot.index == index) return;

            var sendingItem = (EquippableItem)_inventory.PopItemFromSlot(invItem.parentSlot.index);
            if((sendingItem.allowedEquipLocation == type))
            {
                Debug.Log("before swap");
                var swappedItem = _inventory.ReplaceEquipSlot(sendingItem, sendingItem.allowedEquipLocation);
                _inventory.ReplaceItemInSlot(swappedItem, invItem.parentSlot.index);
                Debug.Log("after swap");
            }
        }
    }

    public void DiscardItem()
    {
        _inventory.DropItemFromSlot(type);
    }
}
