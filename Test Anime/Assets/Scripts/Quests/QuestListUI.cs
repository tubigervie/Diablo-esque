using RPG.Questing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestListUI : MonoBehaviour
{
    [SerializeField] QuestItemUI questPrefab;
    [SerializeField] QuestDetailsUI detailsUI;
    QuestList questList;
    // Start is called before the first frame update
    void Start()
    {
        questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
        questList.onUpdate += Redraw;
        Redraw();
    }

    private void Redraw()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (QuestStatus status in questList.GetStatuses())
        {
            QuestItemUI questItemInstance = Instantiate(questPrefab, this.transform);
            questItemInstance.Setup(status, detailsUI);
        }
    }
}
