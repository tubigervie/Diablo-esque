using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryItem : ScriptableObject
{
    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
        Mythical
    }

    [SerializeField] string _itemID = System.Guid.NewGuid().ToString();
    public StatModifier[] statModifiers;
    [SerializeField] float _baseCost;
    [SerializeField] Rarity _rarity;
    [SerializeField] int _level;
    [SerializeField] string _displayName;
    [TextArea]
    [SerializeField] string _description;
    [SerializeField] Sprite _icon;
    [SerializeField] WeaponPickup _pickup;

    public string itemID { get { return _itemID; } }
    public Sprite icon { get { return _icon; } }
    public string displayName { get { return _displayName; } }
    public string description { get { return _description; } }

    public WeaponPickup SpawnPickup(Vector3 position)
    {
        var pickup = Instantiate(_pickup);
        pickup.wasFromInventory = true;
        pickup.transform.position = position;
        pickup.weapon = this;
        return pickup;
    }
}
