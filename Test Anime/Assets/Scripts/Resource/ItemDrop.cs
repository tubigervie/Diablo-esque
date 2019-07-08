using RPG.Resource;
using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [System.Serializable]
    public class Drop
    {
        public InventoryItem item;
        public float dropPercentage;
    }

    public BaseStats stats;
    public Drop[] drops;

    // Start is called before the first frame update
    private void Awake()
    {
        stats = GetComponent<BaseStats>();
    }

    private void OnEnable()
    {
        GetComponent<Health>().onDie += DropItem;
    }

    private void OnDisable()
    {
        GetComponent<Health>().onDie -= DropItem;
    }

    void DropItem()
    {
        Drop highestThreshold = null;
        float rng = Random.Range(0f, 100f);
        Debug.Log("rng: " + rng);
        for(int i = 0; i < drops.Length; i++)
        {
            if (drops[i].dropPercentage < rng) continue;
            if(highestThreshold == null)
            {
                highestThreshold = drops[i];
                continue;
            }
            else if(drops[i].dropPercentage < highestThreshold.dropPercentage)
            {
                highestThreshold = drops[i];
                Debug.Log(highestThreshold.item.displayName + " is now the thrshold");
                continue;
            }
        }
        if (highestThreshold == null) return;

        ItemPickup drop = highestThreshold.item.SpawnPickup(transform.position);
        Vector3 rotation = drop.transform.eulerAngles;
        rotation.y = Random.Range(0, 360);
        drop.transform.eulerAngles = rotation;
        drop.InitRandom(stats.GetLevel(), true);
    }
}
