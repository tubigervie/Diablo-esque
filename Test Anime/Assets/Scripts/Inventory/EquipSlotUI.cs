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
    ItemInstance _item;

    public Inventory inventory { set { _inventory = value; } }

    public EquippableItem.EquipLocation type { get => _type; }

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
        if (invItem == null && equipItem != null)
        {
            Debug.Log("Dropped from equipped into equipped");
            if (equipItem.parentSlot.index == index) return;
        }
        else
        {
            Debug.Log("Dropped from inv into equip slot");
            if (invItem.parentSlot.index == index) return;

            ItemInstance itemInst = _inventory.PopItemFromSlot(invItem.parentSlot.index);
            var sendingItem = new EquipInstance(itemInst.itemBase as EquippableItem, itemInst.properties);
            if((sendingItem.equipBase.allowedEquipLocation == type))
            {
                Debug.Log("before swap");
                var swappedItem = _inventory.ReplaceEquipSlot(sendingItem, sendingItem.equipBase.allowedEquipLocation);
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
