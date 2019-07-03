using RPG.Combat;
using RPG.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour, ISaveable
{
    int coins;
    [SerializeField] EquippableItem defaultWeapon;
    //[SerializeField]
    [SerializeField] InventoryItemList inventoryItemList;
    [SerializeField] int inventorySize;
    private InventorySlot[] inventorySlots;
    [SerializeField] EquipSlot weaponSlot;
    List<WeaponPickup> droppedItems = new List<WeaponPickup>();

    [System.Serializable]
    public struct InventorySlot
    {
        public InventoryItem item;
    }

    [System.Serializable]
    public struct EquipSlot
    {
        public EquippableItem.EquipLocation type;
        public EquippableItem item;
    }

    private void Awake()
    {
        if(inventorySlots == null)
            inventorySlots = new InventorySlot[inventorySize];
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

    public EquippableItem PopEquipSlot(EquippableItem.EquipLocation type)
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
                GetComponent<Fighter>().EquipWeapon(defaultWeapon as Weapon);
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
                item = weaponSlot.item;
                var spawnLocation = transform.position;
                SpawnPickup(item, spawnLocation);
                GetComponent<Fighter>().EquipWeapon(defaultWeapon as Weapon);
                weaponSlot.item = null;
                break;
        }
        inventoryUpdated();
    }

    public void SetWeaponSlot(Weapon weapon)
    {
        weaponSlot.item = weapon;
    }

    public bool AddToFirstEmptySlot(InventoryItem item)
    {
        for(int i = 0; i< inventorySlots.Length; i++)
        {
            if(inventorySlots[i].item == null)
            {
                inventorySlots[i].item = item;
                inventoryUpdated();
                return true;
            }
        }
        return false;
    }

    public bool DropItem(int slot)
    {
        var item = PopItemFromSlot(slot);
        if (item == null) return false;

        var spawnLocation = transform.position;
        SpawnPickup(item, spawnLocation);

        return true; ;
    }

    private void SpawnPickup(InventoryItem item, Vector3 spawnLocation)
    {
        var pickup = item.SpawnPickup(spawnLocation);
        droppedItems.Add(pickup);
    }

    public InventoryItem ReplaceItemInSlot(InventoryItem item, int slot)
    {
        var oldItem = inventorySlots[slot].item;
        inventorySlots[slot].item = item;
        inventoryUpdated();
        return oldItem;
    }

    public InventoryItem ReplaceEquipSlot(EquippableItem item, EquippableItem.EquipLocation type)
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
                GetComponent<Fighter>().EquipWeapon(item as Weapon);
                break;
        }
        inventoryUpdated();
        return oldItem;
    }


    public InventoryItem GetItemInSlot(int slot)
    {
        return inventorySlots[slot].item;
    }

    public InventoryItem PopItemFromSlot(int slot)
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
        var slotStrings = new string[inventorySize];
        for (int i = 0; i < inventorySize; i++)
        {
            if (inventorySlots[i].item != null)
                slotStrings[i] = inventorySlots[i].item.itemID;
            else
                slotStrings[i] = null;
        }
        state["inventorySlots"] = slotStrings;

        RemoveDestroyedDrops();

        var droppedItemsList = new Dictionary<string, object>[droppedItems.Count];

        for (int i = 0; i < droppedItemsList.Length; i++)
        {
            droppedItemsList[i] = new Dictionary<string, object>();
            droppedItemsList[i]["itemID"] = droppedItems[i].weapon.itemID;
            droppedItemsList[i]["position"] = new SerializableVector3(droppedItems[i].transform.position);
        }
        state["droppedItems"] = droppedItemsList;
        return state;
    }

    public void RestoreState(object state)
    {
        Dictionary<string, object> stateDict = (Dictionary<string, object>)state;
        string[] slotStrings = (string[])stateDict["inventorySlots"];
        if (inventorySlots == null)
            inventorySlots = new InventorySlot[inventorySize];
        for (int i = 0; i < inventorySize; i++)
        {
            inventorySlots[i].item = inventoryItemList.GetFromID(slotStrings[i]);
        }
        inventoryUpdated();
        DeleteAllDrops();
        if(stateDict.ContainsKey("droppedItems"))
        {
            Dictionary<string, object>[] droppedItems = (Dictionary<string, object>[])stateDict["droppedItems"];
            foreach (var item in droppedItems)
            {
                var pickupItem = inventoryItemList.GetFromID((string)item["itemID"]);
                Vector3 position = ((SerializableVector3)item["position"]).ToVector();
                SpawnPickup(pickupItem, position);
            }
        }
    }
}
