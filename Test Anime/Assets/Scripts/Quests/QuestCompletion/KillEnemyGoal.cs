using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KillEnemyGoal : Goal
{
    [SerializeField] int numberOfEnemiesToKill = 1;
    int enemiesKilled = 0;

    public override void EvalutateCompletion()
    {
        enemiesKilled++;
        if(enemiesKilled >= numberOfEnemiesToKill)
            return;
    }
}
