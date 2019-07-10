using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatTooltip : MonoBehaviour
{
    [SerializeField] Text bodyText;

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

}
