using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    Inventory _playerInventory;
    [SerializeField] InventorySlotUI inventoryItemPrefab;
    [SerializeField] EquipSlotUI equipSlot;

    // Start is called before the first frame update
    void Start()
    {
        var player = GameObject.FindWithTag("Player");
        _playerInventory = player.GetComponent<Inventory>();
        _playerInventory.inventoryUpdated += Redraw;
        Redraw();
        equipSlot.inventory = _playerInventory;
        equipSlot.index = 100;
        equipSlot.SetItem(_playerInventory.GetWeaponSlot().item);
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

        equipSlot.SetItem(_playerInventory.GetWeaponSlot().item);
    }

    public void CloseInventoryUI()
    {

    }
}
