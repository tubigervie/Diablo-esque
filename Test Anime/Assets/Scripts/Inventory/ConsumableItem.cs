using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConsumableItem : InventoryItem
{
    public abstract void Use(GameObject entity);
}
