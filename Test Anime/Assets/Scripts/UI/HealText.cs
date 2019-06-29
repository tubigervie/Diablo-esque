using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealText : MonoBehaviour
{
    [SerializeField] Text text;
    // Start is called before the first frame update
    public void Activate(float amount, Vector3 position)
    {
        transform.position = Camera.main.WorldToScreenPoint(position);
        text.text = amount.ToString("0.0");
    }
}
