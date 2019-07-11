using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomKeyValuePair<Key, Val>
{
    public Key key { get; set; }
    public Val value { get; set; }

    public CustomKeyValuePair() { }

    public CustomKeyValuePair(Key key, Val val)
    {
        this.key = key;
        this.value = val;
    }
}
