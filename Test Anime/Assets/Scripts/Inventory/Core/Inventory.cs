using RPG.Combat;
using RPG.Saving;
using RPG.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour, ISaveable, IModifier
{
    [SerializeField ] int coins;
    [SerializeField] Text coinText;
    [SerializeField] EquippableItem defaultWeaponBase;
    [SerializeField] EquippableItem defaultArmorBase;

    [SerializeField] AudioClip equipSFX;
    [SerializeField] AudioClip unequipSFX;
    [SerializeField] AudioClip pickUpSFX;

    [SerializeField] InventoryItemList inventoryItemList;
    [SerializeField] int inventorySize;

    private InventorySlot[] inventorySlots;
    EquipSlot weaponSlot;
    EquipSlot necklaceSlot;
    EquipSlot gloveSlot;
    EquipSlot bootSlot;
    EquipSlot armorSlot;
    EquipSlot headSlot;

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
        inventorySlots = new InventorySlot[inventorySize];
        for(int i = 0; i < inventorySize; i++)
        {
            inventorySlots[i].item = null;
        }
    }

    void Start()
    {
        Appearance appearance = GetComponent<Appearance>();
        if (armorSlot.item == null && appearance.currentBodyPrefab == null)
        {
            ArmorInstance defaultArmorInstance = new ArmorInstance(defaultArmorBase as Armor, 1);
            if (appearance.isMale)
                appearance.StartCoroutine(appearance.EquipBody(defaultArmorInstance.armorBase.equippedMalePrefab));
            else
                appearance.StartCoroutine(appearance.EquipBody(defaultArmorInstance.armorBase.equippedFemalePrefab));
        }
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

    public EquipSlot GetBootSlot()
    {
        return bootSlot;
    }

    public EquipSlot GetArmorSlot()
    {
        return armorSlot;
    }

    public EquipSlot GetHeadSlot()
    {
        return headSlot;
    }

    public ItemInstance PopEquipSlot(EquippableItem.EquipLocation type)
    {
        switch (type)
        {
            case EquippableItem.EquipLocation.Helmet:
                return headSlot.item;
            case EquippableItem.EquipLocation.Amulet:
                return necklaceSlot.item;
            case EquippableItem.EquipLocation.Body:
                return armorSlot.item;
            case EquippableItem.EquipLocation.Gloves:
                return gloveSlot.item;
            case EquippableItem.EquipLocation.Boots:
                return bootSlot.item;
            case EquippableItem.EquipLocation.Weapon:
                return weaponSlot.item;
        }
        return null;
    }

    public void RemoveEquippedItem(EquippableItem.EquipLocation type)
    {
        switch (type)
        {
            case EquippableItem.EquipLocation.Helmet:
                headSlot.item = null;
                Appearance headAppearance = GetComponent<Appearance>();
                headAppearance.RemoveHead();
                break;
            case EquippableItem.EquipLocation.Amulet:
                necklaceSlot.item = null;
                break;
            case EquippableItem.EquipLocation.Body:
                armorSlot.item = null;
                ArmorInstance defaultArmorInstance = new ArmorInstance(defaultArmorBase as Armor, 1);
                Appearance appearance = GetComponent<Appearance>();
                if (appearance.isMale)
                    appearance.StartCoroutine(appearance.EquipBody(defaultArmorInstance.armorBase.equippedMalePrefab));
                else
                    appearance.StartCoroutine(appearance.EquipBody(defaultArmorInstance.armorBase.equippedFemalePrefab));
                break;
            case EquippableItem.EquipLocation.Gloves:
                gloveSlot.item = null;
                break;
            case EquippableItem.EquipLocation.Boots:
                bootSlot.item = null;
                break;
            case EquippableItem.EquipLocation.Weapon:
                WeaponInstance defaultWeaponInstance = new WeaponInstance(defaultWeaponBase as Weapon, 1);
                GetComponent<Fighter>().EquipWeapon(defaultWeaponInstance);
                weaponSlot.item = null;
                break;
        }
        GetComponent<AudioSource>().PlayOneShot(unequipSFX);
        inventoryUpdated();
    }

    public void DropItemFromSlot(EquippableItem.EquipLocation type)
    {
        Vector3 spawnLocation;
        switch (type)
        {
            case EquippableItem.EquipLocation.Helmet:
                spawnLocation = transform.position;
                SpawnPickup(headSlot.item, spawnLocation);
                headSlot.item = null;
                Appearance headAppearance = GetComponent<Appearance>();
                headAppearance.RemoveHead();
                break;
            case EquippableItem.EquipLocation.Amulet:
                spawnLocation = transform.position;
                SpawnPickup(necklaceSlot.item, spawnLocation);
                necklaceSlot.item = null;
                break;
            case EquippableItem.EquipLocation.Body:
                spawnLocation = transform.position;
                SpawnPickup(armorSlot.item, spawnLocation);
                armorSlot.item = null;
                ArmorInstance defaultArmorInstance = new ArmorInstance(defaultArmorBase as Armor, 1);
                Appearance appearance = GetComponent<Appearance>();
                if (appearance.isMale)
                    appearance.StartCoroutine(appearance.EquipBody(defaultArmorInstance.armorBase.equippedMalePrefab));
                else
                    appearance.StartCoroutine(appearance.EquipBody(defaultArmorInstance.armorBase.equippedFemalePrefab));
                break;
            case EquippableItem.EquipLocation.Gloves:
                spawnLocation = transform.position;
                SpawnPickup(gloveSlot.item, spawnLocation);
                gloveSlot.item = null;
                break;
            case EquippableItem.EquipLocation.Boots:
                spawnLocation = transform.position;
                SpawnPickup(bootSlot.item, spawnLocation);
                bootSlot.item = null;
                break;
            case EquippableItem.EquipLocation.Weapon:
                spawnLocation = transform.position;
                SpawnPickup(weaponSlot.item, spawnLocation);
                WeaponInstance defaultWeaponInstance = new WeaponInstance(defaultWeaponBase as Weapon, 1);
                GetComponent<Fighter>().EquipWeapon(defaultWeaponInstance);
                weaponSlot.item = null;
                break;
        }
        GetComponent<AudioSource>().PlayOneShot(unequipSFX);
        inventoryUpdated();
    }

    //For adding in items as quest reward
    public void AddItemIntoInventory(ItemInstance item)
    {
        inventoryItemList.AddToInventoryItemList(item.itemBase);
        AddToFirstEmptySlot(item);
    }

    public void PickUpItem(ItemInstance item)
    {
        inventoryItemList.AddToInventoryItemList(item.itemBase);
        GetComponent<AudioSource>().PlayOneShot(pickUpSFX);
        AddToFirstEmptySlot(item);
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
        GetComponent<AudioSource>().PlayOneShot(unequipSFX);
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

    public EquipInstance ReplaceEquipSlot(EquipInstance item, EquippableItem.EquipLocation type, bool isReplacing = true)
    {
        EquipInstance oldItem;
        switch (type)
        {
            case EquippableItem.EquipLocation.Helmet:
                oldItem =  headSlot.item;
                headSlot.item = item;
                ArmorInstance headInst = new ArmorInstance(item.equipBase as Armor, item.properties);
                GetComponent<Appearance>().EquipHead(headInst.armorBase.equippedMalePrefab, headInst.armorBase.enableHair);
                break;
            case EquippableItem.EquipLocation.Amulet:
                oldItem = necklaceSlot.item;
                necklaceSlot.item = item;
                break;
            case EquippableItem.EquipLocation.Body:
                oldItem = armorSlot.item;
                armorSlot.item = item;
                ArmorInstance armInst = new ArmorInstance(item.equipBase as Armor, item.properties);
                Appearance appearance = GetComponent<Appearance>();
                if(appearance.isMale)
                    appearance.StartCoroutine(appearance.EquipBody(armInst.armorBase.equippedMalePrefab));
                else
                    appearance.StartCoroutine(appearance.EquipBody(armInst.armorBase.equippedFemalePrefab));
                break;
            case EquippableItem.EquipLocation.Gloves:
                oldItem = gloveSlot.item;
                gloveSlot.item = item;
                break;
            case EquippableItem.EquipLocation.Boots:
                oldItem = bootSlot.item;
                bootSlot.item = item;
                break;
            case EquippableItem.EquipLocation.Weapon:
                oldItem = weaponSlot.item;
                weaponSlot.item = item;
                WeaponInstance weapInst = new WeaponInstance(item.equipBase as Weapon, item.properties);
                GetComponent<Fighter>().EquipWeapon(weapInst);
                break;
            default:
                oldItem = null;
                break;
        }
        if (isReplacing)
            GetComponent<AudioSource>().PlayOneShot(equipSFX);

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
        int initial = coins;
        coins += amount;
        StartCoroutine(ChangeCoinText(initial, coins));
    }

    IEnumerator ChangeCoinText(int initial, int final)
    {
        for(int i = initial; i <= final; i++)
        {
            coinText.text = i.ToString();
            yield return null;
        }
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
            state["necklaceSlot"] = "";

        if (gloveSlot.item != null)
            state["gloveSlot"] = new KeyValuePair<string, ItemProperties>(gloveSlot.item.itemID, gloveSlot.item.properties);
        else
            state["gloveSlot"] = "";

        if (bootSlot.item != null)
            state["bootSlot"] = new KeyValuePair<string, ItemProperties>(bootSlot.item.itemID, bootSlot.item.properties);
        else
            state["bootSlot"] = "";

        if (armorSlot.item != null)
            state["armorSlot"] = new KeyValuePair<string, ItemProperties>(armorSlot.item.itemID, armorSlot.item.properties);
        else
            state["armorSlot"] = defaultArmorBase.itemID;

        if (headSlot.item != null)
            state["headSlot"] = new KeyValuePair<string, ItemProperties>(headSlot.item.itemID, headSlot.item.properties);
        else
            state["headSlot"] = "";

        RemoveDestroyedDrops();
        //ArmorInstance headInst = new ArmorInstance(item.equipBase as Armor, item.properties);
        //GetComponent<Appearance>().EquipHead(headInst.armorBase.equippedMalePrefab, headInst.armorBase.hideHair);
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
                Debug.Log(slotStrings[i].Key);
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
        if (stateDict["armorSlot"] != null)
        {
            try
            {
                KeyValuePair<string, ItemProperties> armorPair = (KeyValuePair<string, ItemProperties>)stateDict["armorSlot"];
                EquippableItem equipBase = inventoryItemList.GetFromID(armorPair.Key) as EquippableItem;
                ArmorInstance armInst = new ArmorInstance(equipBase as Armor, armorPair.Value);
                Appearance appearance = GetComponent<Appearance>();
                if (appearance.isMale)
                    appearance.StartCoroutine(appearance.EquipBody(armInst.armorBase.equippedMalePrefab));
                else
                    appearance.StartCoroutine(appearance.EquipBody(armInst.armorBase.equippedFemalePrefab));
                armorSlot.item = armInst;
            }
            catch
            {
                armorSlot.item = null;
                ArmorInstance defaultArmorInstance = new ArmorInstance(defaultArmorBase as Armor, 1);
                Appearance appearance = GetComponent<Appearance>();
                if (appearance.isMale)
                    appearance.StartCoroutine(appearance.EquipBody(defaultArmorInstance.armorBase.equippedMalePrefab));
                else
                    appearance.StartCoroutine(appearance.EquipBody(defaultArmorInstance.armorBase.equippedFemalePrefab));
            }
        }
        if (stateDict["headSlot"] != null)
        {
            try
            {
                KeyValuePair<string, ItemProperties> headPair = (KeyValuePair<string, ItemProperties>)stateDict["headSlot"];
                EquippableItem equipBase = inventoryItemList.GetFromID(headPair.Key) as EquippableItem;
                ArmorInstance headInst = new ArmorInstance(equipBase as Armor, headPair.Value);
                GetComponent<Appearance>().EquipHead(headInst.armorBase.equippedMalePrefab, headInst.armorBase.enableHair);
                headSlot.item = headInst;
            }
            catch
            {
                headSlot.item = null;
                GetComponent<Appearance>().RemoveHead();
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
        if (stateDict["bootSlot"] != null)
        {
            try
            {
                KeyValuePair<string, ItemProperties> bootPair = (KeyValuePair<string, ItemProperties>)stateDict["bootSlot"];
                Armor equipBase = inventoryItemList.GetFromID(bootPair.Key) as Armor;
                EquipInstance boots = new EquipInstance(equipBase, bootPair.Value);
                bootSlot.item = boots;
            }
            catch
            {
                bootSlot.item = null;
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
        if(armorSlot.item != null)
        {

            foreach (var modifier in armorSlot.item.properties.statModifiers)
            {
                if (modifier.stat == stat)
                    yield return armorSlot.item.GetStatBonus(modifier.stat, BonusType.Flat);
            }
            if (stat == Stat.Defense)
            {
                ArmorInstance armorInst = new ArmorInstance(armorSlot.item.equipBase as Armor, armorSlot.item.properties);
                yield return armorInst.defenseValue;
            }
        }
        if (headSlot.item != null)
        {

            foreach (var modifier in headSlot.item.properties.statModifiers)
            {
                if (modifier.stat == stat)
                    yield return headSlot.item.GetStatBonus(modifier.stat, BonusType.Flat);
            }
            if (stat == Stat.Defense)
            {
                ArmorInstance headInst = new ArmorInstance(headSlot.item.equipBase as Armor, headSlot.item.properties);
                yield return headInst.defenseValue;
            }
        }
        if (bootSlot.item != null)
        {

            foreach (var modifier in bootSlot.item.properties.statModifiers)
            {
                if (modifier.stat == stat)
                    yield return bootSlot.item.GetStatBonus(modifier.stat, BonusType.Flat);
            }
            if (stat == Stat.Defense)
            {
                ArmorInstance armorInst = new ArmorInstance(bootSlot.item.equipBase as Armor, bootSlot.item.properties);
                yield return armorInst.defenseValue;
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
        if (armorSlot.item != null)
        {

            foreach (var modifier in armorSlot.item.properties.statModifiers)
            {
                if (modifier.stat == stat)
                    yield return armorSlot.item.GetStatBonus(modifier.stat, BonusType.Percentage);
            }

        }
        if (bootSlot.item != null)
        {
            foreach (var modifier in bootSlot.item.properties.statModifiers)
            {
                if (modifier.stat == stat)
                    yield return bootSlot.item.GetStatBonus(modifier.stat, BonusType.Percentage);
            }
        }
        if (headSlot.item != null)
        {
            foreach (var modifier in headSlot.item.properties.statModifiers)
            {
                if (modifier.stat == stat)
                    yield return headSlot.item.GetStatBonus(modifier.stat, BonusType.Percentage);
            }
        }

    }
}
