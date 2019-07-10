using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StatTooltipSpawner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] StatTooltip tooltipPrefab;

    StatTooltip _tooltip;

    const string strengthDescription = "Strength directly influences the power of your attacks. For every point of strength, you gain an additional .5 bonus to your damage.";
    const string dexterityDescription = "Dexterity affects the precision of your attacks. For every point of dexterity, you gain an additional .2% bonus to your critical hit chance.";
    const string consitutionDescription = "Constitution grants resistance to the blows from your enemies. For every point of constitution, you receive an additional .5 bonus to defense and 10 additional Health points.";
    const string criticalDescription = "Critical hit chance is the probability that you will land a critical hit with your attacks, increasing its damage by 50%.";
    const string healthDescription = "Health is your life force that allows you to withstand attacks from enemies.";
    const string energyDescription = "Energy is your well of spiritual power that allows you to cast powerful abilities.";

    private void PositionTooltip()
    {
        // Required to ensure corners are updated by positioning elements.
        Canvas.ForceUpdateCanvases();

        var tooltipCorners = new Vector3[4];
        _tooltip.GetComponent<RectTransform>().GetWorldCorners(tooltipCorners);
        var slotCorners = new Vector3[4];
        GetComponent<RectTransform>().GetWorldCorners(slotCorners);

        bool below = transform.position.y > Screen.height / 2;
        bool right = transform.position.x < Screen.width / 2;

        int slotCorner = GetCornerIndex(below, right);
        int tooltipCorner = GetCornerIndex(!below, !right);

        _tooltip.transform.position = slotCorners[slotCorner] - tooltipCorners[tooltipCorner] + _tooltip.transform.position;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        Stat stat = GetComponentInParent<StatTypeUI>().statType;

        var parentCanvas = GetComponentInParent<Canvas>();

        if (!_tooltip)
        {
            _tooltip = Instantiate(tooltipPrefab, parentCanvas.transform);
        }

        switch (stat)
        {
            case Stat.Health:
                _tooltip.body = healthDescription;
                break;
            case Stat.Energy:
                _tooltip.body = energyDescription;
                break;
            case Stat.Strength:
                _tooltip.body = strengthDescription;
                break;
            case Stat.Dexterity:
                _tooltip.body = dexterityDescription;
                break;
            case Stat.Constitution:
                _tooltip.body = consitutionDescription;
                break;
            case Stat.CriticalHitChance:
                _tooltip.body = criticalDescription;
                break;
        }
        PositionTooltip();
    }



    private int GetCornerIndex(bool below, bool right)
    {
        if (below && !right) return 0;
        else if (!below && !right) return 1;
        else if (!below && right) return 2;
        else return 3;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ClearTooltip();
    }

    private void ClearTooltip()
    {
        if (_tooltip)
        {
            Destroy(_tooltip.gameObject);
        }
    }
}
