using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquippedAbilitySlotUI : MonoBehaviour
{
    [SerializeField] Image _iconImage;

    AbilityConfig _ability;

    public SetAbilityUI abilityUI;



    public int index;

    public AbilityConfig ability { get => _ability; set { SetAbility(value); } }

    private void SetAbility(AbilityConfig value)
    {
        _ability = value;
        if(value != null)
        {
            _iconImage.sprite = value.GetIcon();
            _iconImage.color = Color.white;
        }
        else
        {
            _iconImage.sprite = null;
            _iconImage.color = Color.black;
        }
    }

    public void SetAbilityBinding()
    {
        abilityUI.SetAbility(index);
    }
}
