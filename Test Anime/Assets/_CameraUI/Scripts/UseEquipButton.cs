using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseEquipButton : MonoBehaviour
{
    public EquipSlotUI equipUI;
    public InventorySlotUI slotUI;

    public void OnClickUseEquip()
    {
        InventorySlotUI slotUI = GetComponentInParent<InventorySlotUI>();
        var player = GameObject.FindWithTag("Player");
        if(slotUI.item.itemBase.isConsummable)
        {
            ConsummableInstance consum = new ConsummableInstance(slotUI.item.itemBase as ConsumableItem, slotUI.item.properties);
            consum.consumBase.Use(player);
            consum.properties.count--;
            Inventory _inventory = player.GetComponent<Inventory>();
            _inventory.ReplaceItemInSlot(consum, slotUI.index);
            slotUI.itemCount.text = consum.properties.count.ToString();
            if (consum.properties.count <= 0)
            {
                _inventory.PopItemFromSlot(slotUI.index);
            }
        }
        else if(slotUI.item.itemBase.isArmor)
        {
            EquipInstance armor = new EquipInstance(slotUI.item.itemBase as EquippableItem, slotUI.item.properties);
            ItemInstance oldItem = slotUI.inventory.ReplaceEquipSlot(armor, armor.equipBase.allowedEquipLocation, true);
            slotUI.inventory.ReplaceItemInSlot(oldItem, slotUI.index);
        }
        else if(slotUI.item.itemBase.isWeapon)
        {
            WeaponInstance weapon = new WeaponInstance(slotUI.item.itemBase as Weapon, slotUI.item.properties);
            ItemInstance oldItem = slotUI.inventory.ReplaceEquipSlot(weapon, weapon.weaponBase.allowedEquipLocation, true);
            slotUI.inventory.ReplaceItemInSlot(oldItem, slotUI.index);
        }
        else
        {
            EquipInstance armor = new EquipInstance(slotUI.item.itemBase as EquippableItem, slotUI.item.properties);
            ItemInstance oldItem = slotUI.inventory.ReplaceEquipSlot(armor, armor.equipBase.allowedEquipLocation, true);
            slotUI.inventory.ReplaceItemInSlot(oldItem, slotUI.index);
        }
        slotUI.DisableButton();
    }

    public void RemoveEquip()
    {
        EquipSlotUI slotUI = GetComponentInParent<EquipSlotUI>();
        ItemInstance equip = slotUI.inventory.PopEquipSlot(slotUI.type);
        slotUI.inventory.RemoveEquippedItem(slotUI.type);
        slotUI.inventory.AddToFirstEmptySlot(equip);
        slotUI.DisableButton();
    }

    public void DisableButton()
    {
        if (slotUI != null)
        {
            slotUI.DisableButton();
        }
        else
        {
            Debug.Log("equip UI: " + equipUI);
            if (equipUI != null)
                equipUI.DisableButton();
        }
    }

}
