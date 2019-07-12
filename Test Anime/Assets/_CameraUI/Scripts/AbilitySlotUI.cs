using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IAbilityHolder
{
    SpecialAbilityList.Ability ability { get; set; }
}

public class AbilitySlotUI : MonoBehaviour, IAbilityHolder
{
    [SerializeField] Image _iconImage;
    [SerializeField] Text _levelText;

    SpecialAbilityList.Ability _ability;

    public AbilityUI abilityUI;

    public SpecialAbilityList.Ability ability { get => _ability; set { SetAbility(value); } }

    public int index;

    private void SetAbility(SpecialAbilityList.Ability value)
    {
        _ability = value;
        _iconImage.sprite = value.config.GetIcon();
        _levelText.text = "Lv " + value.level;
        this.GetComponent<Button>().interactable = true;
    }

    public void EnableAbilitySetter()
    {
        abilityUI.EnableAbilitySetter(index);
    }

    public void DisableAbility()
    {
        _iconImage.color = Color.gray;
        this.GetComponent<Button>().interactable = false;
    }
}
