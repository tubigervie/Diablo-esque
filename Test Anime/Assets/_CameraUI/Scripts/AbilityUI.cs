using RPG.Combat;
using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUI : MonoBehaviour
{
    GameObject player;
    SpecialAbilities _abilities;
    SpecialAbilityList _abilityList;
    [SerializeField] AbilitySlotUI abilitySlotPrefab; //switch to AbilitySlotUI
    [SerializeField] SetAbilityUI abilitySetter;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        _abilities = player.GetComponent<SpecialAbilities>();
        player.GetComponent<BaseStats>().onLevelUp += Redraw;
        _abilityList = _abilities.abilityIndex;
        Redraw();
    }

    private void Redraw()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < _abilityList.allAbilities.Count; i++)
        {
            var slotUI = Instantiate(abilitySlotPrefab, transform);
            slotUI.abilityUI = this;
            slotUI.ability = _abilityList.allAbilities[i];
            slotUI.index = i;
            if (player.GetComponent<BaseStats>().GetLevel() < slotUI.ability.level)
            {
                slotUI.DisableAbility();
            }
        }
    }

    private AbilityConfig GetAbility(int index)
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<AbilitySlotUI>().index == index)
            {
                return child.GetComponent<AbilitySlotUI>().ability.config;
            }
        }
        return null;
    }

    public void EnableAbilitySetter(int index)
    {
        AbilityConfig ability = GetAbility(index);
        abilitySetter.selectedAbility = ability;
        abilitySetter.gameObject.SetActive(true);
    }

    public void DisableAbilitySetter()
    {
        abilitySetter.DisableAbilitySetter();
    }

}
