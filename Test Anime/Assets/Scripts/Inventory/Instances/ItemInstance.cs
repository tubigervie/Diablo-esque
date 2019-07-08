using RPG.Stats;
using System;
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
    public float count;
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
        //this.properties.statModifiers.AddRange(item.statModifiers);

         CreateStatModifiers(random, item);
        this.properties.instanceID = System.Guid.NewGuid().ToString();
        this.properties.count = 1;
        this.itemID = item.itemID;
        this.itemBase = item;
    }

    private void CreateStatModifiers(System.Random random, InventoryItem item)
    {
        for (int i = 0; i < (int)this.properties.rarity; i++)
        {
            List<Stat> values = new List<Stat>();
            foreach (StatModifier stat in item.statModifiers)
            {
                StatModifier findStat = this.properties.statModifiers.Find(P => P.stat == stat.stat);
                if (findStat == null)
                    values.Add(stat.stat);
            }
            if (values == null) break;

            StatModifier newStat = new StatModifier();

            newStat.stat = values[random.Next(0, values.Count)];
            StatModifier statMax = item.statModifiers.Find(P => P.stat == newStat.stat);
            newStat.bonusType = statMax.bonusType;
            newStat.maxAmount = UnityEngine.Random.Range(statMax.minAmount, statMax.maxAmount);

            switch (this.properties.rarity)
            {
                case Rarity.Common:
                    break;
                case Rarity.Uncommon:
                    newStat.maxAmount *= 1.1f;
                    break;
                case Rarity.Rare:
                    newStat.maxAmount *= 1.2f;
                    break;
                case Rarity.Epic:
                    newStat.maxAmount *= 1.35f;
                    break;
                case Rarity.Legendary:
                    newStat.maxAmount *= 1.5f;
                    break;
                case Rarity.Mythical:
                    newStat.maxAmount *= 2f;
                    break;
            }
            newStat.maxAmount = (float)Math.Round(newStat.maxAmount, 1);
            newStat.minAmount = newStat.maxAmount;

            this.properties.statModifiers.Add(newStat);
        }




    }

    public ItemInstance(InventoryItem item, ItemProperties properties)
    {
        this.properties.rarity = properties.rarity;
        this.properties.level = properties.level;
        this.properties.statModifiers = properties.statModifiers;
        this.properties.instanceID = properties.instanceID;
        this.properties.count = properties.count;
        this.itemID = item.itemID;
        this.itemBase = item;
    }

    public float GetStatBonus(Stat bonus, BonusType type)
    {
        foreach (StatModifier modifier in properties.statModifiers)
        {
            if (modifier.stat == bonus && modifier.bonusType == type)
                return modifier.maxAmount;
        }
        return 0;
    }

}