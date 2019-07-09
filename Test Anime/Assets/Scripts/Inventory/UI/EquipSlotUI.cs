using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipSlotUI : MonoBehaviour, IItemHolder, IDropHandler, IPointerClickHandler
{
    [SerializeField] Image _iconImage;
    [SerializeField] EquippableItem.EquipLocation _type;
    [SerializeField] GameObject useButton;

    bool isActive = false;

    public InventoryUI invUI;
    public int index { get; set; }

    Inventory _inventory;
    ItemInstance _item;

    public Inventory inventory { get { return _inventory; } set { _inventory = value; } }

    public EquippableItem.EquipLocation type { get => _type; }

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
        this.useButton.GetComponent<UseEquipButton>().equipUI = this;
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

            if (invItem == null) return;
            if (invItem.parentSlot.index == index) return;
            ItemInstance itemInst = _inventory.PopItemFromSlot(invItem.parentSlot.index);
            if (itemInst.itemBase.isConsummable)
            {
                _inventory.AddItemToSlot(invItem.parentSlot.index, itemInst);
                return;
            }
            var sendingItem = new EquipInstance(itemInst.itemBase as EquippableItem, itemInst.properties);
            if ((sendingItem.equipBase.allowedEquipLocation == type))
            {
                var swappedItem = _inventory.ReplaceEquipSlot(sendingItem, sendingItem.equipBase.allowedEquipLocation);
                _inventory.ReplaceItemInSlot(swappedItem, invItem.parentSlot.index);
            }
            else
                _inventory.AddItemToSlot(invItem.parentSlot.index, sendingItem);
        }
    }

    public void DiscardItem()
    {
        _inventory.DropItemFromSlot(type);
    }

    public void DisableButton()
    {
        if (invUI.activeButton != null)
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
            if (invUI.activeButton == this.useButton && isActive)
            {
                DisableButton();
            }
            else
            {
                this.useButton.GetComponentInChildren<Text>().text = "Remove";
                invUI.activeButton = this.useButton;
                invUI.activeButton.SetActive(true);
                isActive = true;
            }
        }
    }
}
