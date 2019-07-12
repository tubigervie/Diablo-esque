using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("RPG/Ability/Index"))]
public class SpecialAbilityList : ScriptableObject
{
    [System.Serializable]
    public struct Ability
    {
        public int level;
        public AbilityConfig config;
    }

    public List<Ability> allAbilities;
    

}
