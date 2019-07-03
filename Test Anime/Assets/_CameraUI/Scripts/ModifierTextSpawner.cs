using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModifierTextSpawner : MonoBehaviour
{
    [SerializeField] GameObject modifierTextPrefab;

    public void Spawn(BonusType bonus, float amount, Stat stat)
    {
        GameObject text = Instantiate(modifierTextPrefab, this.transform);
        Text valueText = text.GetComponent<Text>();

        switch (bonus)
        {
            case BonusType.Flat:
                valueText.text = "+" + amount + " " + stat;
                break;
            case BonusType.Percentage:
                valueText.text = "+" + amount + "% " + stat;
                break;
        }
    }
}
