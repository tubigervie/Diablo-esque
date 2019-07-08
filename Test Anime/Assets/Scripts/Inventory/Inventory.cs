using RPG.Combat;
using RPG.Saving;
using RPG.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour, ISaveable, IModifier
{
    int coins;

    [SerializeField] EquippableItem defaultWeaponBase;
    [SerializeField] EquippableItem defaultAmuletBase;
    [SerializeField] EquippableItem defaultGloveBase;

    [SerializeField] AudioClip equipSFX;
    [SerializeField] InventoryItemList inventoryItemList;
    [SerializeField] int inventorySize;
    private InventorySlot[] inventorySlots;
    EquipSlot weaponSlot;
    EquipSlot necklaceSlot;
    EquipSlot gloveSlot;
    List<ItemPickup> droppedItems = new List<ItemPickup>();

    [System.Serializable]
    public struct InventorySlot
    {
        public ItemInstance item;
    }

    [System.Serializable]
    public struct EquipSlot
    {
        public EquippableItem.EquipLocation type;
        public EquipInstance item;
    }

    private void Awake()
    {
        //if(inventorySlots == null)
        inventorySlots = new InventorySlot[inventorySize];
        for(int i = 0; i < inventorySize; i++)
        {
            inventorySlots[i].item = null;
        }
        //inventorySlots[0].item = inventoryItemList.GetFromID("82e243bc-95fb-4ed5-bc4a-db34bc4530ca");
    }

    private void Start()
    {
    }

    public event Action inventoryUpdated = delegate { };

    public InventorySlot[] slots
    {
        get { return inventorySlots; }
    }

    public EquipSlot GetWeaponSlot()
    {
        return weaponSlot;
    }

    public EquipSlot GetGloveSlot()
    {
        return gloveSlot;
    }

    public ItemInstance PopEquipSlot(EquippableItem.EquipLocation type)
    {
        switch (type)
        {
            case EquippableItem.EquipLocation.Helmet:
                return weaponSlot.item;
            case EquippableItem.EquipLocation.Amulet:
                return necklaceSlot.item;
            case EquippableItem.EquipLocation.Body:
                return weaponSlot.item;
            case EquippableItem.EquipLocation.Gloves:
                return gloveSlot.item;
            case EquippableItem.EquipLocation.Boots:
                return weaponSlot.item;
            case EquippableItem.EquipLocation.Weapon:
                return weaponSlot.item;
        }
        return weaponSlot.item;
    }

    public void RemoveEquippedItem(EquippableItem.EquipLocation type)
    {
        switch (type)
        {
            case EquippableItem.EquipLocation.Helmet:
                break;
            case EquippableItem.EquipLocation.Amulet:
                necklaceSlot.item = null;
                break;
            case EquippableItem.EquipLocation.Body:
                break;
            case EquippableItem.EquipLocation.Gloves:
                gloveSlot.item = null;
                break;
            case EquippableItem.EquipLocation.Boots:
                break;
            case EquippableItem.EquipLocation.Weapon:
                WeaponInstance defaultWeaponInstance = new WeaponInstance(defaultWeaponBase as Weapon, 1);
                GetComponent<Fighter>().EquipWeapon(defaultWeaponInstance);
                weaponSlot.item = null;
                break;
        }
        inventoryUpdated();
    }

    public void DropItemFromSlot(EquippableItem.EquipLocation type)
    {
        Vector3 spawnLocation;
        switch (type)
        {
            case EquippableItem.EquipLocation.Helmet:
                break;
            case EquippableItem.EquipLocation.Amulet:
                spawnLocation = transform.position;
                SpawnPickup(necklaceSlot.item, spawnLocation);
                necklaceSlot.item = null;
                break;
            case EquippableItem.EquipLocation.Body:
                break;
            case EquippableItem.EquipLocation.Gloves:
                spawnLocation = transform.position;
                SpawnPickup(gloveSlot.item, spawnLocation);
                gloveSlot.item = null;
                break;
            case EquippableItem.EquipLocation.Boots:
                break;
            case EquippableItem.EquipLocation.Weapon:
                spawnLocation = transform.position;
                SpawnPickup(weaponSlot.item, spawnLocation);
                WeaponInstance defaultWeaponInstance = new WeaponInstance(defaultWeaponBase as Weapon, 1);
                GetComponent<Fighter>().EquipWeapon(defaultWeaponInstance);
                weaponSlot.item = null;
                break;
        }
        inventoryUpdated();
    }

    public void AddItemIntoInventory(ItemInstance weapon)
    {
        inventoryItemList.AddToInventoryItemList(weapon.itemBase);
        AddToFirstEmptySlot(weapon);
    }

    public void SetWeaponSlot(EquipInstance weapon)
    {
        weaponSlot.item = weapon;
    }

    public void RemoveDroppedItem(ItemPickup pickup)
    {
        droppedItems.Remove(pickup);
    }

    public bool AddToFirstEmptySlot(ItemInstance item)
    {
        for(int i = 0; i< inventorySlots.Length; i++)
        {
            if(inventorySlots[i].item != null && inventorySlots[i].item.itemBase == item.itemBase && item.itemBase.isConsummable)
            {
                ConsummableInstance dropInstance = new ConsummableInstance(item.itemBase as ConsumableItem, item.properties);
                ConsummableInstance invInstance = new ConsummableInstance(inventorySlots[i].item.itemBase as ConsumableItem, inventorySlots[i].item.properties);
                invInstance.properties.count += dropInstance.properties.count;
                inventorySlots[i].item = invInstance;
                inventoryUpdated();
                return true;
            }
            else if (inventorySlots[i].item == null || inventorySlots[i].item.itemBase == null)
            {
                inventorySlots[i].item = item;
                inventoryUpdated();
                return true;
            }
        }
        Debug.Log("couldnt find empty");
        return false;
    }

    public bool DropItem(int slot)
    {
        var item = PopItemFromSlot(slot);
        if (item == null) return false;

        var spawnLocation = transform.position;
        SpawnPickup(item, spawnLocation);

        return true; 
    }

    private void SpawnPickup(ItemInstance item, Vector3 spawnLocation)
    {
        var pickup = item.itemBase.SpawnPickup(spawnLocation, item);
        droppedItems.Add(pickup);
    }

    public ItemInstance ReplaceItemInSlot(ItemInstance item, int slot)
    {
        var oldItem = inventorySlots[slot].item;
        inventorySlots[slot].item = item;
        inventoryUpdated();
        return oldItem;
    }

    public EquipInstance ReplaceEquipSlot(EquipInstance item, EquippableItem.EquipLocation type)
    {
        EquipInstance oldItem;
        switch (type)
        {
            case EquippableItem.EquipLocation.Helmet:
                oldItem = necklaceSlot.item;
                weaponSlot.item = item;
                break;
            case EquippableItem.EquipLocation.Amulet:
                oldItem = necklaceSlot.item;
                necklaceSlot.item = item;
                GetComponent<AudioSource>().PlayOneShot(equipSFX);
                break;
            case EquippableItem.EquipLocation.Body:
                oldItem = necklaceSlot.item;
                weaponSlot.item = item;
                break;
            case EquippableItem.EquipLocation.Gloves:
                oldItem = gloveSlot.item;
                gloveSlot.item = item;
                GetComponent<AudioSource>().PlayOneShot(equipSFX);
                break;
            case EquippableItem.EquipLocation.Boots:
                oldItem = necklaceSlot.item;
                weaponSlot.item = item;
                break;
            case EquippableItem.EquipLocation.Weapon:
                oldItem = weaponSlot.item;
                weaponSlot.item = item;
                WeaponInstance weapInst = new WeaponInstance(item.equipBase as Weapon, item.properties);
                GetComponent<Fighter>().EquipWeapon(weapInst);
                GetComponent<AudioSource>().PlayOneShot(equipSFX);
                break;
            default:
                oldItem = weaponSlot.item;
                break;
        }
        inventoryUpdated();
        return oldItem;
    }


    public ItemInstance GetItemInSlot(int slot)
    {
        return inventorySlots[slot].item;
    }

    public ItemInstance PopItemFromSlot(int slot)
    {
        var item = inventorySlots[slot].item;
        inventorySlots[slot].item = null;
        inventoryUpdated();
        return item;
    }

    public bool AddItemToSlot(int slot, ItemInstance item)
    {
        if (inventorySlots[slot].item != null) return false; 
        inventorySlots[slot].item = item;
        inventoryUpdated();
        return true;
    }

    private void DeleteAllDrops()
    {
        RemoveDestroyedDrops();
        foreach (var item in droppedItems)
        {
            Destroy(item.gameObject);
        }
    }

    private void RemoveDestroyedDrops()
    {
        var newList = new List<ItemPickup>();
        foreach (var item in droppedItems)
        {
            if (item != null)
            {
                newList.Add(item);
            }
        }
        droppedItems = newList;
    }

    public void AddCoin(int amount)
    {
        coins += amount;
    }

    public int GetCoinAmount()
    {
        return coins;
    }

    public EquipSlot GetNecklaceSlot()
    {
        return necklaceSlot;
    }

    public object CaptureState()
    {
        Dictionary<string, object> state = new Dictionary<string, object>();
        var slotValues = new KeyValuePair<string, ItemProperties>[inventorySize];
        for (int i = 0; i < inventorySize; i++)
        {
            if (inventorySlots[i].item != null && inventorySlots[i].item.itemBase != null)
                slotValues[i] = new KeyValuePair<string, ItemProperties>(inventorySlots[i].item.itemBase.itemID, inventorySlots[i].item.properties);
            else
            {
                slotValues[i] = new KeyValuePair<string, ItemProperties>();
                Debug.Log("slot key: " + slotValues[i].Key);
            }
        }
        state["inventorySlots"] = slotValues;
        if (weaponSlot.item != null)
            state["weaponSlot"] = new KeyValuePair<string, ItemProperties>(weaponSlot.item.itemID, weaponSlot.item.properties);
        else
            state["weaponSlot"] = defaultWeaponBase.itemID;
        if (necklaceSlot.item != null)
            state["necklaceSlot"] = new KeyValuePair<string, ItemProperties>(necklaceSlot.item.itemID, necklaceSlot.item.properties);
        else
            state["necklaceSlot"] = defaultAmuletBase.itemID;
        if (gloveSlot.item != null)
            state["gloveSlot"] = new KeyValuePair<string, ItemProperties>(gloveSlot.item.itemID, gloveSlot.item.properties);
        else
            state["gloveSlot"] = defaultGloveBase.itemID;

        RemoveDestroyedDrops();

        var droppedItemsList = new Dictionary<string, object>[droppedItems.Count];

        for (int i = 0; i < droppedItemsList.Length; i++)
        {
            droppedItemsList[i] = new Dictionary<string, object>();
            droppedItemsList[i]["itemID"] = droppedItems[i].itemInstance.itemID;
            droppedItemsList[i]["itemProperties"] = droppedItems[i].itemInstance.properties;
            droppedItemsList[i]["position"] = new SerializableVector3(droppedItems[i].transform.position);
        }
        state["droppedItems"] = droppedItemsList;
        return state;
    }

    public void RestoreState(object state)
    {
        Dictionary<string, object> stateDict = (Dictionary<string, object>)state;
        KeyValuePair<string, ItemProperties>[] slotStrings = (KeyValuePair<string, ItemProperties>[])stateDict["inventorySlots"];
        inventorySlots = new InventorySlot[inventorySize];
        for (int i = 0; i < inventorySize; i++)
        {
            inventorySlots[i].item = null;
        }
        for (int i = 0; i < inventorySize; i++)
        {
            if(!String.IsNullOrEmpty(slotStrings[i].Key))
            {
                InventoryItem itemBase = inventoryItemList.GetFromID(slotStrings[i].Key);
                inventorySlots[i].item = new ItemInstance(itemBase, slotStrings[i].Value);
            }
        }
        if (stateDict["weaponSlot"] != null)
        {
            try
            {
                KeyValuePair<string, ItemProperties> weaponPair = (KeyValuePair<string, ItemProperties>)stateDict["weaponSlot"];
                EquippableItem equipBase = inventoryItemList.GetFromID(weaponPair.Key) as EquippableItem;
                WeaponInstance weapon = new WeaponInstance(equipBase as Weapon, weaponPair.Value);
                GetComponent<Fighter>().EquipWeapon(weapon);
                weaponSlot.item = weapon;
            }
            catch
            {
                string weaponBase = (string)stateDict["weaponSlot"];
                EquippableItem equipBase = inventoryItemList.GetFromID(weaponBase) as EquippableItem;
                WeaponInstance weapon = new WeaponInstance(equipBase as Weapon, 1);
                GetComponent<Fighter>().EquipWeapon(weapon);
                weaponSlot.item = null;
            }
        }
        if (stateDict["necklaceSlot"] != null)
        {
            try
            {
                KeyValuePair<string, ItemProperties> necklacePair = (KeyValuePair<string, ItemProperties>)stateDict["necklaceSlot"];
                EquippableItem equipBase = inventoryItemList.GetFromID(necklacePair.Key) as EquippableItem;
                EquipInstance necklace = new EquipInstance(equipBase as EquippableItem, necklacePair.Value);
                necklaceSlot.item = necklace;
            }
            catch
            {
                necklaceSlot.item = null;
            }
        }
        if (stateDict["gloveSlot"] != null)
        {
            try
            {
                KeyValuePair<string, ItemProperties> glovePair = (KeyValuePair<string, ItemProperties>)stateDict["gloveSlot"];
                EquippableItem equipBase = inventoryItemList.GetFromID(glovePair.Key) as EquippableItem;
                EquipInstance gloves = new EquipInstance(equipBase as EquippableItem, glovePair.Value);
                gloveSlot.item = gloves;
            }
            catch
            {
                gloveSlot.item = null;
            }
        }

        inventoryUpdated();
        DeleteAllDrops();
        if (stateDict.ContainsKey("droppedItems"))
        {
            Dictionary<string, object>[] droppedItems = (Dictionary<string, object>[])stateDict["droppedItems"];
            foreach (var item in droppedItems)
            {
                var pickupItem = inventoryItemList.GetFromID((string)item["itemID"]);
                ItemInstance instancedItem = new ItemInstance(pickupItem, (ItemProperties)item["itemProperties"]);
                Vector3 position = ((SerializableVector3)item["position"]).ToVector();
                SpawnPickup(instancedItem, position);
            }
        }
    }

    public IEnumerable<float> GetAdditiveModifier(Stat stat)
    {
        if (necklaceSlot.item != null)
        {
            foreach (var modifier in necklaceSlot.item.properties.statModifiers)
            {
                if (modifier.stat == stat)
                    yield return necklaceSlot.item.GetStatBonus(modifier.stat, BonusType.Flat);
            }
        }
        if (gloveSlot.item != null)
        {
            foreach (var modifier in gloveSlot.item.properties.statModifiers)
            {
                if (modifier.stat == stat)
                    yield return gloveSlot.item.GetStatBonus(modifier.stat, BonusType.Flat);
            }
        }

    }

    public IEnumerable<float> GetPercentageModifier(Stat stat)
    {
        if (necklaceSlot.item != null)
        {
            foreach (var modifier in necklaceSlot.item.properties.statModifiers)
            {
                if (modifier.stat == stat)
                    yield return necklaceSlot.item.GetStatBonus(modifier.stat, BonusType.Percentage);
            }
        }
        if (gloveSlot.item != null)
        {
            foreach (var modifier in gloveSlot.item.properties.statModifiers)
            {
                if (modifier.stat == stat)
                    yield return gloveSlot.item.GetStatBonus(modifier.stat, BonusType.Percentage);
            }
        }

    }
}
