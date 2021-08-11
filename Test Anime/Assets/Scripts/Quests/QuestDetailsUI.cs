using RPG.Questing;
using System;
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
    [SerializeField] TextMeshProUGUI rewardText;

    void Start()
    {
        ToggleOff();
    }

    public void Setup(QuestStatus status)
    {
        title.text = status.GetQuest().GetTitle();
        objectiveContainer.DetachChildren();
        foreach (var objective in status.GetQuest().GetObjectives())
        {
            GameObject prefab = (status.IsObjectiveComplete(objective.reference)) ? objectivePrefab : objectiveIncompletePrefab;
            GameObject objectiveInstance = Instantiate(prefab, objectiveContainer);
            TextMeshProUGUI objectiveText = objectiveInstance.GetComponentInChildren<TextMeshProUGUI>();
            objectiveText.text = objective.description;
        }
        rewardText.text = GetRewardText(status.GetQuest());
    }

    private string GetRewardText(Quest quest)
    {
        string rewardText = "";
        foreach(var reward in quest.GetRewards())
        {
            if(rewardText != "")
            {
                rewardText += ", ";
            }
            if(reward.number > 1)
            {
                rewardText += reward.number + " ";
            }
            rewardText += reward.item.GetDisplayName();
        }
        if(rewardText == "")
        {
            rewardText = "No reward";
        }
        return rewardText;
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
