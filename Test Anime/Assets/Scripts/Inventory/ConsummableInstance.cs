using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsummableInstance : ItemInstance
{
    public ConsumableItem consumBase;

    public ConsummableInstance(ConsumableItem item, int level) : base(item, level)
    {
        this.consumBase = item;
    }

    public ConsummableInstance(ConsumableItem item, ItemProperties properties) : base(item, properties)
    {
        this.consumBase = item;
    }
}
