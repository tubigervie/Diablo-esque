using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BonusType { Flat, Percentage}

[System.Serializable]
public class StatModifier
{
    public Stat stat;
    public BonusType bonusType;
    public float minAmount;
    public float maxAmount;
}
