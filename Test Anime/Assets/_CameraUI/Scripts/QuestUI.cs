using RPG.Questing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    GameObject player;
    Journal _journal;
    [SerializeField] QuestText questTextPrefab; //replace with QuestTextUI;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        _journal = player.GetComponent<Journal>();
        Redraw();
        _journal.journalChanged += Redraw;
    }


    void Redraw()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        foreach(Quest quest in _journal.activeQuests)
        {
            var GO = Instantiate(questTextPrefab, transform);
            GO.titleText.text = quest.displayName;
            GO.descriptionText.text = quest.questDescription;
        }
    }
}
