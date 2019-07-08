using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("RPG/Inventory/Index"))]
public class InventoryItemList : ScriptableObject
{
    [SerializeField] List<InventoryItem> allInventoryItems;

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

    public void AddToInventoryItemList(InventoryItem inventoryItem)
    {
        if(!allInventoryItems.Contains(inventoryItem))
            allInventoryItems.Add(inventoryItem);
    }


    public void RemoveByID(string itemID)
    {
        foreach (var item in allInventoryItems)
        {
            if (item.itemID == itemID)
            {
                allInventoryItems.Remove(item);
            }
        }
    }
}
