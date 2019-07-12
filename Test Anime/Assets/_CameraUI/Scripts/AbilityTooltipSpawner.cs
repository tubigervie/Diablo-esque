using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AbilityTooltipSpawner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] AbilityTooltip tooltipPrefab;

    AbilityTooltip _tooltip;
    GameObject player;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }


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
        SpecialAbilityList.Ability ability = GetComponent<IAbilityHolder>().ability;

        var parentCanvas = GetComponentInParent<Canvas>();

        if (!_tooltip)
        {
            _tooltip = Instantiate(tooltipPrefab, parentCanvas.transform);
        }

        _tooltip.body = ability.config.GetAbilityQuote();
        _tooltip.title = ability.config.GetAbilityName();
        if (player.GetComponent<BaseStats>().GetLevel() < ability.level)
            _tooltip.levelUnlock = "Requires Lv " + ability.level + " to use.";
        else
            _tooltip.levelUnlock = "";
        _tooltip.effects = ability.config.GetAbilityDescription();
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
