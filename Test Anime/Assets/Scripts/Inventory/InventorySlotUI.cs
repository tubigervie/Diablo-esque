using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public interface IItemHolder
{
    ItemInstance item { get; set; }
}

public class InventorySlotUI : MonoBehaviour, IItemHolder, IDropHandler, IPointerClickHandler
{
    [SerializeField] Image _iconImage;
    public Text itemCount;
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
            if (item.itemBase.isConsummable)
            {
                ConsummableInstance consum = new ConsummableInstance(item.itemBase as ConsumableItem, item.properties);
                itemCount.text = consum.properties.count.ToString();
            }
            else
                itemCount.text = "";
        }
        else
        {
            _iconImage.enabled = false;
            itemCount.text = "";
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        var invItem = eventData.pointerDrag.GetComponent<ItemInventoryUI>();
        var equipItem = eventData.pointerDrag.GetComponent<ItemEquippedUI>();
        if (invItem == null && equipItem == null) return;
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
            if(swappedItem != null && sendingItem.itemBase != null && swappedItem.itemBase != null && sendingItem.itemBase.isConsummable && swappedItem.itemBase == sendingItem.itemBase)
            {
                ConsummableInstance sendingConsum = new ConsummableInstance(sendingItem.itemBase as ConsumableItem, sendingItem.properties);
                ConsummableInstance consum = new ConsummableInstance(swappedItem.itemBase as ConsumableItem, swappedItem.properties);
                consum.properties.count += sendingConsum.properties.count;
                _inventory.ReplaceItemInSlot(consum, index);
                item = consum;
                itemCount.text = consum.properties.count.ToString();
            }
            else
                _inventory.ReplaceItemInSlot(swappedItem, invItem.parentSlot.index);
        }
        
    }

    public void DiscardItem()
    {
        _inventory.DropItem(index);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(item != null && item.itemBase != null && item.itemBase.isConsummable)
        {
            var player = GameObject.FindWithTag("Player");
            ConsummableInstance consum = new ConsummableInstance(item.itemBase as ConsumableItem, item.properties);
            consum.consumBase.Use(player);
            consum.properties.count--;
            _inventory.ReplaceItemInSlot(consum, index);
            itemCount.text = consum.properties.count.ToString();
            if (consum.properties.count <= 0)
            {
                _inventory.PopItemFromSlot(index);
            }

        }
    }
}
