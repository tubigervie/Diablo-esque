using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AI Action Database", menuName = "RPG/AI Action Database")]
public class AIActionDatabase : ScriptableObject
{
    public List<ActionCondition> ActionConditions = new List<ActionCondition>();
}

