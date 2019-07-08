using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorInstance : EquipInstance
{
    public Armor armorBase;
    public float defenseValue;

    public ArmorInstance(Armor item, int level) : base(item, level)
    {
        this.armorBase = item;
        this.defenseValue = this.armorBase.defenseValue;
        float defenseMultiplier = 1;
        switch (this.properties.rarity)
        {
            case Rarity.Common:
                defenseMultiplier = 1;
                break;
            case Rarity.Uncommon:
                defenseMultiplier = 1.2f;
                break;
            case Rarity.Rare:
                defenseMultiplier = 1.4f;
                break;
            case Rarity.Epic:
                defenseMultiplier = 1.5f;
                break;
            case Rarity.Legendary:
                defenseMultiplier = 1.8f;
                break;
            case Rarity.Mythical:
                defenseMultiplier = 2f;
                break;
        }
        this.defenseValue *= defenseMultiplier;
    }

    public ArmorInstance(Armor item, ItemProperties properties) : base(item, properties)
    {
        this.armorBase = item;
        this.defenseValue = this.armorBase.defenseValue;
        float defenseMultiplier = 1;
        switch (this.properties.rarity)
        {
            case Rarity.Common:
                defenseMultiplier = 1;
                break;
            case Rarity.Uncommon:
                defenseMultiplier = 1.2f;
                break;
            case Rarity.Rare:
                defenseMultiplier = 1.4f;
                break;
            case Rarity.Epic:
                defenseMultiplier = 1.5f;
                break;
            case Rarity.Legendary:
                defenseMultiplier = 1.8f;
                break;
            case Rarity.Mythical:
                defenseMultiplier = 2f;
                break;
        }
        this.defenseValue *= defenseMultiplier;
    }
}
