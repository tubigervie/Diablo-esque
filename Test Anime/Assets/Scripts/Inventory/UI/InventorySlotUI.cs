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
    [SerializeField] GameObject useButton;
    bool isActive = false;

    public Text itemCount;
    public int index { get; set; }

    Inventory _inventory;
    ItemInstance _item;

    public InventoryUI invUI;

    public Inventory inventory { get { return _inventory; } set { _inventory = value; } }

    public ItemInstance item
    {
        get => _item;
        set
        {
            SetItem(value);
        }
    }

    void Start()
    {
        this.useButton.GetComponent<UseEquipButton>().slotUI = this;
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
                EquipInstance sentInst = new EquipInstance(sendingItem.itemBase as EquippableItem, sendingItem.properties);

                if (swappedItem.itemBase.isConsummable)
                {
                    _inventory.ReplaceEquipSlot(sentInst, equipItem.parentSlot.type, false);
                    _inventory.ReplaceItemInSlot(swappedItem, index);
                }

                EquipInstance equipInst = new EquipInstance(swappedItem.itemBase as EquippableItem, swappedItem.properties);
                if(equipInst.equipBase.allowedEquipLocation == sentInst.equipBase.allowedEquipLocation)
                    _inventory.ReplaceEquipSlot(equipInst, equipItem.parentSlot.type);
                else
                {
                    _inventory.ReplaceEquipSlot(sentInst, equipItem.parentSlot.type, false);
                    _inventory.ReplaceItemInSlot(equipInst, index);
                }
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

    public void DisableButton()
    {
        if(invUI.activeButton != null)
        {
            invUI.activeButton.SetActive(false);
            isActive = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (invUI.activeButton != null && invUI.activeButton != this.useButton)
        {
            DisableButton();
            invUI.activeButton = null;
        }
        if (item != null && item.itemBase != null)
        {
            if(invUI.activeButton == this.useButton && isActive)
            {
                DisableButton();
            }
            else
            {
                //if non usable put right here
                if(item.itemBase.isConsummable)
                    this.useButton.GetComponentInChildren<Text>().text = "Use";
                else if(item.itemBase.isArmor || item.itemBase.isWeapon)
                    this.useButton.GetComponentInChildren<Text>().text = "Equip";
                else
                    this.useButton.GetComponentInChildren<Text>().text = "Equip";
                invUI.activeButton = this.useButton;
                invUI.activeButton.SetActive(true);
                isActive = true;
            }
        }
    }
}
