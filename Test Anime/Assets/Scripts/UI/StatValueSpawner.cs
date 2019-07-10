using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StatValueSpawner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Stat statType;
    [SerializeField] GameObject healthGameObject;
    [SerializeField] GameObject energyGameObject;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Entered");
        switch (statType)
        {
            case Stat.Health:
                healthGameObject.SetActive(true);
                break;
            case Stat.Energy:
                energyGameObject.SetActive(true);
                break;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        switch (statType)
        {
            case Stat.Health:
                healthGameObject.SetActive(false);
                break;
            case Stat.Energy:
                energyGameObject.SetActive(false);
                break;
        }
    }
}
