using RPG.Questing;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestDetailsUI : MonoBehaviour
{
    [SerializeField] GameObject questList;
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] Transform objectiveContainer;
    [SerializeField] GameObject objectivePrefab;
    [SerializeField] GameObject objectiveIncompletePrefab;

    void Start()
    {
        ToggleOff();
    }

    public void Setup(QuestStatus status)
    {
        title.text = status.GetQuest().GetTitle();
        objectiveContainer.DetachChildren();
        foreach (string objective in status.GetQuest().GetObjectives())
        {
            GameObject prefab = (status.IsObjectiveComplete(objective)) ? objectivePrefab : objectiveIncompletePrefab;
            GameObject objectiveInstance = Instantiate(prefab, objectiveContainer);
            TextMeshProUGUI objectiveText = objectiveInstance.GetComponentInChildren<TextMeshProUGUI>();
            objectiveText.text = objective;
        }
    }

    public void ToggleOff()
    {
        questList.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void ToggleOn()
    {
        this.gameObject.SetActive(true);
    }
}
