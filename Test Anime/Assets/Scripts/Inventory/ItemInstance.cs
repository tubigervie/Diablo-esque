using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public struct ItemProperties
{
    public Rarity rarity;
    public int level;
    public List<StatModifier> statModifiers;
    public string instanceID;
}


[System.Serializable]
public class ItemInstance
{
    public string itemID;
    public ItemProperties properties;
    public InventoryItem itemBase;

    public ItemInstance(InventoryItem item, int level)
    {
        System.Random random = new System.Random();
        this.properties.rarity = item.possibleRarityValues[random.Next(0, item.possibleRarityValues.Length)];
        this.properties.level = level;
        this.properties.statModifiers = new List<StatModifier>();
        this.properties.statModifiers.AddRange(item.statModifiers);
        this.properties.instanceID = System.Guid.NewGuid().ToString();
        this.itemID = item.itemID;
        this.itemBase = item;
    }

    public ItemInstance(InventoryItem item, ItemProperties properties)
    {
        this.properties.rarity = properties.rarity;
        this.properties.level = properties.level;
        this.properties.statModifiers = properties.statModifiers;
        this.properties.instanceID = properties.instanceID;
        this.itemID = item.itemID;
        this.itemBase = item;
    }

}
