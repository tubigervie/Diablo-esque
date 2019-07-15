using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Dialogue
{
    [System.Serializable]
    public struct DialogueEventBinding
    {
        public string name;
        public UnityEvent callback;
    }

}
