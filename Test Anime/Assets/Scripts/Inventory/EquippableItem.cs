using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("RPG/Inventory/Equipable Item"))]
public class EquippableItem : InventoryItem
{
    public enum EquipLocation
    {
        Helmet,
        Amulet,
        Body,
        Gloves,
        Boots,
        Weapon,
    }

    [SerializeField] EquipLocation _allowedEquipLocation;

    public EquipLocation allowedEquipLocation
    {
        get
        {
            return _allowedEquipLocation;
        }
    }

}
