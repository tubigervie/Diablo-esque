using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public interface IItemHolder
{
    ItemInstance item { get; set; }
}

public class InventorySlotUI : MonoBehaviour, IItemHolder, IDropHandler
{
    [SerializeField] Image _iconImage;

    public int index { get; set; }

    Inventory _inventory;
    ItemInstance _item;

    public Inventory inventory { set { _inventory = value; } }

    public ItemInstance item
    {
        get => _item;
        set
        {
            SetItem(value);
        }
    }

    public void SetItem(ItemInstance item)
    {
        _item = item;

        if (item != null && item.itemBase != null)
        {
            _iconImage.enabled = true;
            _iconImage.sprite = item.itemBase.icon;
        }
        else
        {
            _iconImage.enabled = false;
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
            Debug.Log("UUID: " + sendingItem.itemID);
            var swappedItem = _inventory.ReplaceItemInSlot(sendingItem, index);
            if (swappedItem == null || swappedItem.itemBase == null)
                _inventory.RemoveEquippedItem(equipItem.parentSlot.type);
            else
            {
                Debug.Log("swapped Item or item base is not null");
                EquipInstance equipInst = new EquipInstance(swappedItem.itemBase as EquippableItem, swappedItem.properties);
                _inventory.ReplaceEquipSlot(equipInst, equipItem.parentSlot.type);
            }
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
