using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatTypeUI : MonoBehaviour
{
    public Stat statType;
    [SerializeField] Text _statTypeText;
    [SerializeField] Text _statValue;

    public Text statTypeText { get { return _statTypeText; } }
    public Text statValue { get { return _statValue; } set { _statValue = value; } }


    // Start is called before the first frame update
    void Start()
    {
        switch (statType)
        {
            case Stat.Health:
                statTypeText.text = "Max Health";
                break;
            case Stat.Energy:
                statTypeText.text = "Max Energy";
                break;
            case Stat.Strength:
                statTypeText.text = "Strength";
                break;
            case Stat.Dexterity:
                statTypeText.text = "Dexterity";
                break;
            case Stat.Constitution:
                statTypeText.text = "Constitution";
                break;
            case Stat.CriticalHitChance:
                statTypeText.text = "Critical Hit Chance";
                break;
        }
    }
}
