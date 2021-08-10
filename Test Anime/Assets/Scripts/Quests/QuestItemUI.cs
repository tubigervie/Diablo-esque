using RPG.Questing;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestItemUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI progress;
    QuestStatus _currentQuestStatus;
    QuestDetailsUI detailsUI;
    
    public void Setup(QuestStatus status, QuestDetailsUI details)
    {
        detailsUI = details;
        _currentQuestStatus = status;
        title.text = _currentQuestStatus.GetQuest().GetTitle();
        progress.text = status.GetCompletedCount() + "/" + _currentQuestStatus.GetQuest().GetObjectiveCount();
    }

    public void OnClickQuest()
    {
        detailsUI.Setup(_currentQuestStatus);
        detailsUI.ToggleOn();
    }
}
