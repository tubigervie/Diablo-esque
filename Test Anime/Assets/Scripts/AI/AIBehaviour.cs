using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AI Behaviour", menuName = "RPG/AI")]
public class AIBehaviour : ScriptableObject
{
    [Header("Basic Settings")]
    public float stoppingDistance = 1.5f;
}
