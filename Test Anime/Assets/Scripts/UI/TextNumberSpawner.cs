using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextNumberSpawner : MonoBehaviour
{
    [SerializeField] GameObject damageTextPrefab;
    [SerializeField] GameObject healTextPrefab;
    [SerializeField] GameObject critTextPrefab;
    [SerializeField] Vector3 textOffset;

    public void CreateDamageText(float amount, Vector3 location)
    {
        var instance = Instantiate(damageTextPrefab);
        instance.transform.SetParent(transform);
        var damageText = instance.GetComponent<DamageText>();
        damageText.Activate(amount, location + textOffset);
    }

    public void CreateHealText(float amount, Vector3 location)
    {
        var instance = Instantiate(healTextPrefab);
        instance.transform.SetParent(transform);
        var healText = instance.GetComponent<HealText>();
        healText.Activate(amount, location + textOffset);
    }

    public void CreateCritText(float amount, Vector3 location)
    {
        var instance = Instantiate(critTextPrefab);
        instance.transform.SetParent(transform);
        var critText = instance.GetComponent<CritText>();
        critText.Activate(amount, location + textOffset);
    }
}
