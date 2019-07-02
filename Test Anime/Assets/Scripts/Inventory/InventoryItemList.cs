using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("RPG/Inventory/Index"))]
public class InventoryItemList : ScriptableObject
{
    [SerializeField] InventoryItem[] allInventoryItems;

    public InventoryItem GetFromID(string itemID)
    {
        foreach (var item in allInventoryItems)
        {
            if (item.itemID == itemID)
            {
                return item;
            }
        }

        return null;
    }
}
