using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary,
    Mythical
}


public abstract class InventoryItem : ScriptableObject
{

    [SerializeField] string _itemID = System.Guid.NewGuid().ToString();
    public List<StatModifier> statModifiers;
    [SerializeField] float _baseCost;
    [SerializeField] Rarity[] _rarity;
    [SerializeField] int _level;
    [SerializeField] string _displayName;
    [TextArea]
    [SerializeField] string _description;
    [SerializeField] Sprite _icon;
    [SerializeField] ItemPickup _pickup;

    public string itemID { get { return _itemID; } }
    public Sprite icon { get { return _icon; } }
    public string displayName { get { return _displayName; } }
    public string description { get { return _description; } }
    public Rarity[] possibleRarityValues { get { return _rarity; } }

    public ItemPickup SpawnPickup(Vector3 position)
    {
        var pickup = Instantiate(_pickup);
        pickup.wasFromInventory = true;
        pickup.transform.position = position;
        pickup.item = this;
        return pickup;
    }

    public ItemPickup SpawnPickup(Vector3 position, ItemInstance item)
    {
        position.y += .25f;
        var pickup = Instantiate(_pickup);
        pickup.InitInstance(item);
        pickup.wasFromInventory = true;
        pickup.transform.position = position;
        return pickup;
    }
}
