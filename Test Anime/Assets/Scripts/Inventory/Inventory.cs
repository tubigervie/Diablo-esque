using RPG.Combat;
using RPG.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour, ISaveable
{
    int coins;
    [SerializeField] EquippableItem defaultWeaponBase;
    [SerializeField] AudioClip equipSFX;
    [SerializeField] InventoryItemList inventoryItemList;
    [SerializeField] int inventorySize;
    private InventorySlot[] inventorySlots;
    [SerializeField] EquipSlot weaponSlot;
    List<WeaponPickup> droppedItems = new List<WeaponPickup>();

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

    public ItemInstance PopEquipSlot(EquippableItem.EquipLocation type)
    {
        switch (type)
        {
            case EquippableItem.EquipLocation.Helmet:
                return weaponSlot.item;
            case EquippableItem.EquipLocation.Amulet:
                return weaponSlot.item;
            case EquippableItem.EquipLocation.Body:
                return weaponSlot.item;
            case EquippableItem.EquipLocation.Pants:
                return weaponSlot.item;
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
                break;
            case EquippableItem.EquipLocation.Body:
                break;
            case EquippableItem.EquipLocation.Pants:
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
        EquippableItem item;
        switch (type)
        {
            case EquippableItem.EquipLocation.Helmet:
                break;
            case EquippableItem.EquipLocation.Amulet:
                break;
            case EquippableItem.EquipLocation.Body:
                break;
            case EquippableItem.EquipLocation.Pants:
                break;
            case EquippableItem.EquipLocation.Boots:
                break;
            case EquippableItem.EquipLocation.Weapon:
                item = weaponSlot.item.itemBase as EquippableItem;
                var spawnLocation = transform.position;
                //SpawnPickup(item, spawnLocation);
                WeaponInstance defaultWeaponInstance = new WeaponInstance(defaultWeaponBase as Weapon, 1);
                GetComponent<Fighter>().EquipWeapon(defaultWeaponInstance);
                weaponSlot.item = null;
                break;
        }
        inventoryUpdated();
    }

    public void AddItemIntoInventory(ItemInstance weapon)
    {
        //inventoryItemList.AddToInventoryItemList(weapon);
        AddToFirstEmptySlot(weapon);
    }

    public void SetWeaponSlot(EquipInstance weapon)
    {
        weaponSlot.item = weapon;
    }

    public bool AddToFirstEmptySlot(ItemInstance item)
    {
        for(int i = 0; i< inventorySlots.Length; i++)
        {
            if(inventorySlots[i].item == null || inventorySlots[i].item.itemBase == null)
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
        //SpawnPickup(item, spawnLocation);

        return true; 
    }

    //private void SpawnPickup(ItemInstance item, Vector3 spawnLocation)
    //{
    //    var pickup = item.SpawnPickup(spawnLocation);
    //    droppedItems.Add(pickup);
    //}

    public ItemInstance ReplaceItemInSlot(ItemInstance item, int slot)
    {
        var oldItem = inventorySlots[slot].item;
        inventorySlots[slot].item = item;
        inventoryUpdated();
        return oldItem;
    }

    public EquipInstance ReplaceEquipSlot(EquipInstance item, EquippableItem.EquipLocation type)
    {
        var oldItem = weaponSlot.item;
        switch (type)
        {
            case EquippableItem.EquipLocation.Helmet:
                weaponSlot.item = item;
                break;
            case EquippableItem.EquipLocation.Amulet:
                weaponSlot.item = item;
                break;
            case EquippableItem.EquipLocation.Body:
                weaponSlot.item = item;
                break;
            case EquippableItem.EquipLocation.Pants:
                weaponSlot.item = item;
                break;
            case EquippableItem.EquipLocation.Boots:
                weaponSlot.item = item;
                break;
            case EquippableItem.EquipLocation.Weapon:
                weaponSlot.item = item;
                WeaponInstance weapInst = new WeaponInstance(item.equipBase as Weapon, item.properties);
                GetComponent<Fighter>().EquipWeapon(weapInst);
                GetComponent<AudioSource>().PlayOneShot(equipSFX);
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
        var newList = new List<WeaponPickup>();
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
        //RemoveDestroyedDrops();

        //var droppedItemsList = new Dictionary<string, object>[droppedItems.Count];

        //for (int i = 0; i < droppedItemsList.Length; i++)
        //{
        //    droppedItemsList[i] = new Dictionary<string, object>();
        //    droppedItemsList[i]["itemID"] = droppedItems[i].weapon.itemID;
        //    droppedItemsList[i]["position"] = new SerializableVector3(droppedItems[i].transform.position);
        //}
        //state["droppedItems"] = droppedItemsList;
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
        else
        {            
        }
        inventoryUpdated();
        //DeleteAllDrops();
        //if(stateDict.ContainsKey("droppedItems"))
        //{
        //    Dictionary<string, object>[] droppedItems = (Dictionary<string, object>[])stateDict["droppedItems"];
        //    foreach (var item in droppedItems)
        //    {
        //        var pickupItem = inventoryItemList.GetFromID((string)item["itemID"]);
        //        Vector3 position = ((SerializableVector3)item["position"]).ToVector();
        //        //SpawnPickup(pickupItem, position);
        //    }
        //}
    }
}
