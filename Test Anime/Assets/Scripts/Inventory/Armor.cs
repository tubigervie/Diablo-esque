using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Armor", menuName = "Armor/Make New Armor", order = 0)]
public class Armor : EquippableItem
{
    public float defenseValue;
    public GameObject equippedMalePrefab = null;
    public GameObject equippedFemalePrefab = null;
}
