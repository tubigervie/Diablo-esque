using RPG.Combat;
using RPG.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    Inventory _playerInventory;
    [SerializeField] InventorySlotUI inventoryItemPrefab;
    [SerializeField] EquipSlotUI weaponSlot;
    [SerializeField] EquipSlotUI necklaceSlot;
    [SerializeField] EquipSlotUI gloveSlot;
    [SerializeField] Text damageText;
    [SerializeField] Text defenseText;

    // Start is called before the first frame update
    void Start()
    {
        var player = GameObject.FindWithTag("Player");
        _playerInventory = player.GetComponent<Inventory>();
        _playerInventory.inventoryUpdated += Redraw;
        player.GetComponent<BaseStats>().onLevelUp += Redraw;
        Redraw();
        weaponSlot.inventory = _playerInventory;
        weaponSlot.index = 100;
        weaponSlot.SetItem(_playerInventory.GetWeaponSlot().item);
        necklaceSlot.inventory = _playerInventory;
        necklaceSlot.index = 200;
        necklaceSlot.SetItem(_playerInventory.GetNecklaceSlot().item);
        gloveSlot.inventory = _playerInventory;
        gloveSlot.index = 300;
        gloveSlot.SetItem(_playerInventory.GetGloveSlot().item);
    }


    private void Redraw()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < _playerInventory.slots.Length; i++)
        {
            var itemUI = Instantiate(inventoryItemPrefab, transform);
            itemUI.inventory = _playerInventory;
            itemUI.index = i;
            itemUI.SetItem(_playerInventory.slots[i].item);
        }

        weaponSlot.SetItem(_playerInventory.GetWeaponSlot().item);
        necklaceSlot.SetItem(_playerInventory.GetNecklaceSlot().item);
        gloveSlot.SetItem(_playerInventory.GetGloveSlot().item);
        BaseStats stats = _playerInventory.gameObject.GetComponent<BaseStats>();
        float damageBonus = stats.GetStat(Stat.Damage) + stats.GetStrengthDamageBonus();
        Fighter fighter = _playerInventory.gameObject.GetComponent<Fighter>();
        float damageMin = (float) Math.Round(fighter.GetDamageRange().min + damageBonus, 1);
        float damageMax = (float)Math.Round(fighter.GetDamageRange().max + damageBonus, 1);
        damageText.text = damageMin.ToString() + " - " + damageMax.ToString();
        defenseText.text = ((float)Math.Round(stats.GetDefense())).ToString();
    }

    public void CloseInventoryUI()
    {

    }
}
