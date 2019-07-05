using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipInstance : ItemInstance
{
    public EquippableItem equipBase;

    public EquipInstance(EquippableItem item, int level) : base(item, level)
    {
        this.equipBase = item;
    }

    public EquipInstance(EquippableItem item, ItemProperties properties) : base(item, properties)
    {
        this.equipBase = item;
    }
}
