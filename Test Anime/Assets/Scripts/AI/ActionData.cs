using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AI Action Data", menuName = "RPG/AI")]
public class ActionData : ScriptableObject
{
    [Header("Parameters")]
    public string coroutineName = string.Empty;
}
