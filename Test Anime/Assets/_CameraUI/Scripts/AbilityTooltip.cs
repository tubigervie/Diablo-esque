using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityTooltip : MonoBehaviour
{
    [SerializeField] Text titleText;
    [SerializeField] Text bodyText;
    [SerializeField] Text levelUnlockText;
    [SerializeField] Text effectText;

    public string body
    {
        set
        {
            bodyText.text = value;
        }
        get
        {
            return bodyText.text;
        }
    }

    public string effects
    {
        set
        {
            effectText.text = value;
        }
        get
        {
            return effectText.text;
        }
    }

    public string title
    {
        set
        {
            titleText.text = value;
        }
        get
        {
            return titleText.text;
        }
    }

    public string levelUnlock
    {
        set
        {
            levelUnlockText.text = value;
        }
        get
        {
            return levelUnlockText.text;
        }
    }

}
