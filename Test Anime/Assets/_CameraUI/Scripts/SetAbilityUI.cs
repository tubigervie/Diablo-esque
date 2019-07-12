using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAbilityUI : MonoBehaviour
{
    GameObject player;
    SpecialAbilities _abilities;
    [SerializeField] EquippedAbilitySlotUI ability1;
    [SerializeField] EquippedAbilitySlotUI ability2;
    [SerializeField] EquippedAbilitySlotUI ability3;
    [SerializeField] EquippedAbilitySlotUI ability4;

    public AbilityConfig selectedAbility = null;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        _abilities = player.GetComponent<SpecialAbilities>();
        Redraw();
    }

    //private void OnEnable()
    //{
    //    Redraw();
    //}

    private void Redraw()
    {
        _abilities = player.GetComponent<SpecialAbilities>();
        ability1.ability = _abilities.GetAbilityAt(0);
        ability1.index = 0;
        ability1.abilityUI = this;
        ability2.ability = _abilities.GetAbilityAt(1);
        ability2.index = 1;
        ability2.abilityUI = this;
        ability3.ability = _abilities.GetAbilityAt(2);
        ability3.index = 2;
        ability3.abilityUI = this;
        ability4.ability = _abilities.GetAbilityAt(3);
        ability4.index = 3;
        ability4.abilityUI = this;
    }

    public void DisableAbilitySetter()
    {
        selectedAbility = null;
        this.gameObject.SetActive(false);
    }

    public void SetAbility(int index)
    {
        if (selectedAbility == null) return;
        _abilities.SwitchAbilityAtIndex(selectedAbility, index);
        DisableAbilitySetter();
        Redraw();
    }

}
