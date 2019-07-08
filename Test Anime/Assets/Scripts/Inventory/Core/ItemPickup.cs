using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickup : MonoBehaviour
{
    Canvas canvas;
    [SerializeField] protected GameObject pivot;
    [SerializeField] protected Text itemName;
    [SerializeField] GameObject legendaryParticlePrefab;
    [SerializeField] GameObject mythicalParticlePrefab;
    [SerializeField] GameObject epicParticlePrefab;
    public InventoryItem item = null;
    public ItemInstance itemInstance;
    public bool wasFromInventory;
    [SerializeField] float respawnTime = 5;
    Camera _camera;

    GameObject currentParticle;

    private void Awake()
    {
        canvas = GetComponentInChildren<Canvas>();
        canvas.worldCamera = Camera.main;
        _camera = Camera.main;
        InitRandom(1);
    }

    private void Start()
    {
        
    }

    public virtual void InitRandom(int level, bool isDrop = false)
    {
        Destroy(currentParticle);
        itemName.text = item.displayName;
        itemInstance = new ItemInstance(item, 1);
        switch (itemInstance.properties.rarity)
        {
            case Rarity.Common:
                itemName.color = Color.white;
                break;
            case Rarity.Uncommon:
                itemName.color = Color.yellow;
                break;
            case Rarity.Rare:
                itemName.color = Color.green;
                break;
            case Rarity.Epic:
                itemName.color = Color.blue;
                currentParticle = Instantiate(epicParticlePrefab, transform);
                break;
            case Rarity.Legendary:
                itemName.color = Color.cyan;
                currentParticle = Instantiate(legendaryParticlePrefab, transform);
                break;
            case Rarity.Mythical:
                itemName.color = Color.red;
                currentParticle = Instantiate(mythicalParticlePrefab, transform);
                break;
        }
    }

    public virtual void InitInstance(ItemInstance item)
    {
        Destroy(currentParticle);
        itemName.text = item.itemBase.displayName;
        this.item = item.itemBase;
        itemInstance = item;
        switch (itemInstance.properties.rarity)
        {
            case Rarity.Common:
                itemName.color = Color.white;
                break;
            case Rarity.Uncommon:
                itemName.color = Color.yellow;
                break;
            case Rarity.Rare:
                itemName.color = Color.green;
                break;
            case Rarity.Epic:
                itemName.color = Color.blue;
                currentParticle = Instantiate(epicParticlePrefab, transform);
                break;
            case Rarity.Legendary:
                itemName.color = Color.cyan;
                currentParticle = Instantiate(legendaryParticlePrefab, transform);
                break;
            case Rarity.Mythical:
                itemName.color = Color.red;
                currentParticle = Instantiate(mythicalParticlePrefab, transform);
                break;
        }
    }

    private void Update()
    {
        pivot.transform.LookAt(_camera.transform.position);
    }

    public void Hide()
    {
        ShowPickup(false);
    }

    private IEnumerator HideForSeconds(float time)
    {
        ShowPickup(false);
        yield return new WaitForSeconds(time);
        ShowPickup(true);
    }

    private void ShowPickup(bool shouldShow)
    {
        GetComponent<Collider>().enabled = shouldShow;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(shouldShow);
        }
    }

    public void UpdatePosition()
    {
        transform.position = transform.position;
    }
}
