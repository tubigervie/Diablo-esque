using RPG.Combat;
using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInstance : EquipInstance
{
    public Weapon weaponBase;
    public DamageRange damageRange;


    public WeaponInstance(Weapon item, int level) : base(item, level)
    {
        this.weaponBase = item;
        float damageMultiplier = 1;
        switch (this.properties.rarity)
        {
            case Rarity.Common:
                damageMultiplier = 1;
                break;
            case Rarity.Uncommon:
                damageMultiplier = 1.1f;
                break;
            case Rarity.Rare:
                damageMultiplier = 1.2f;
                break;
            case Rarity.Epic:
                damageMultiplier = 1.35f;
                break;
            case Rarity.Legendary:
                damageMultiplier = 1.5f;
                break;
            case Rarity.Mythical:
                damageMultiplier = 2f;
                break;
        }
        this.damageRange = item.GetDamageRange() * damageMultiplier;
    }

    public WeaponInstance(Weapon item, ItemProperties properties) : base(item, properties)
    {
        this.weaponBase = item;
        float damageMultiplier = 1;
        switch (this.properties.rarity)
        {
            case Rarity.Common:
                damageMultiplier = 1;
                break;
            case Rarity.Uncommon:
                damageMultiplier = 1.1f;
                break;
            case Rarity.Rare:
                damageMultiplier = 1.2f;
                break;
            case Rarity.Epic:
                damageMultiplier = 1.35f;
                break;
            case Rarity.Legendary:
                damageMultiplier = 1.5f;
                break;
            case Rarity.Mythical:
                damageMultiplier = 2f;
                break;
        }
        this.damageRange = item.GetDamageRange() * damageMultiplier;
    }

    public float GetWeaponDamage()
    {
        return damageRange.RandomlyChooseDamage();
    }

    public DamageRange GetDamageRange()
    {
        return damageRange;
    }

    public float GetStatBonus(Stat bonus, BonusType type)
    {
        foreach (StatModifier modifier in properties.statModifiers)
        {
            if (modifier.stat == bonus && modifier.bonusType == type)
                return modifier.amount;
        }
        return 0;
    }
}
