using RPG.Combat;
using RPG.Resource;
using RPG.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatSheetUI : MonoBehaviour
{
    public StatTypeUI[] stats;
    BaseStats _stats;
    // Start is called before the first frame update
    void Start()
    {
        var player = GameObject.FindWithTag("Player");
        _stats = player.GetComponent<BaseStats>();
        player.GetComponent<Inventory>().inventoryUpdated += Redraw;
        player.GetComponent<BaseStats>().onLevelUp += Redraw;
        Redraw();
    }

    private void Redraw()
    {
        foreach(StatTypeUI stat in stats)
        {
            float statValue = _stats.GetStat(stat.statType);
            switch (stat.statType)
            {
                case Stat.CriticalHitChance:
                    statValue += _stats.GetDexCritChanceBonus();
                    stat.statValue.text = Math.Round(statValue, 1).ToString() + "%";
                    break;
                case Stat.Health:
                    statValue += _stats.GetConstitutionHealthBonus();
                    stat.statValue.text = Math.Round(statValue, 1).ToString();
                    break;
                default:
                    stat.statValue.text = Math.Round(statValue, 1).ToString();
                    break;
            }
        }
    }
}