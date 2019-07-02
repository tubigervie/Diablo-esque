using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public interface IItemHolder
{
    InventoryItem item { get; set; }
}

public class InventorySlotUI : MonoBehaviour, IItemHolder, IDropHandler
{
    [SerializeField] Image _iconImage;

    public int index { get; set; }

    Inventory _inventory;
    InventoryItem _item;

    public Inventory inventory { set { _inventory = value; } }

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
        if (invItem == null)
        {
            Debug.Log("Dropped from equipped into inv");
            var sendingItem = _inventory.PopEquipSlot(equipItem.parentSlot.type);
            var swappedItem = _inventory.ReplaceItemInSlot(sendingItem, index);
            if (swappedItem == null)
                _inventory.RemoveEquippedItem(equipItem.parentSlot.type);
            //_inventory.AddToFirstEmptySlot(swappedItem);
        }
        else
        {
            Debug.Log("Dropped from inv into inv");
            if (invItem.parentSlot.index == index) return;

            var sendingItem = _inventory.PopItemFromSlot(invItem.parentSlot.index);
            var swappedItem = _inventory.ReplaceItemInSlot(sendingItem, index);
            _inventory.ReplaceItemInSlot(swappedItem, invItem.parentSlot.index);
        }
        
    }

    public void DiscardItem()
    {
        _inventory.DropItem(index);
    }
}
