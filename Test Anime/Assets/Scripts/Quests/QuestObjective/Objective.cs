using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Questing
{
    [System.Serializable]
    public class Objective
    {
        public enum ObjectiveType { Kill, Talk, Go};
        public ObjectiveType type;
    }

}
