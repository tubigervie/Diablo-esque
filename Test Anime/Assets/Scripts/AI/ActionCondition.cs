using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AI Condition")]
public class ActionCondition : ScriptableObject
{
    public AbilityConfig abilityConfig;

    [Header("Stat Type")]
    [Range(0f, 100f)]
    public float minHealthPercent;

    [Range(0f, 100f)]
    public float maxHealthPercent;

    public Vector2 actionWaitTimeRange = new Vector2(0.5f, 1f);

    public float minDistance;

    public float maxDistance;
}
