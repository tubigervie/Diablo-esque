using RPG.Questing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    public class Voice : MonoBehaviour
    {
        [SerializeField] string npcID;
        [SerializeField] Conversation conversation;
        [SerializeField] [Tooltip("Optional")] string questID;
        [Space(15)]
        [SerializeField] Transform canvas;
        [SerializeField] GameObject speechBubblePrefab;
        DialogueDisplay dialogueDisplay;
        GameObject speechBubble;

        public List<DialogueEventBinding> dialogueEventBindings;

        public Conversation GetConversation()
        {
            return conversation;
        }

        public string GetNPCName()
        {
            return npcID;
        }


        public void TriggerEventForAction(string action)
        {
            foreach (var mapping in dialogueEventBindings)
            {
                if(mapping.name == action)
                {
                    mapping.callback.Invoke();
                }
            }
        }




        // Start is called before the first frame update
        void Start()
        {
            speechBubble = Instantiate(speechBubblePrefab, canvas);
            dialogueDisplay = FindObjectOfType<DialogueDisplay>();
        }

        public void TriggerQuestIfAny()
        {
            if (questID == null) return;
            var journal = FindObjectOfType<Journal>();
            var quest = journal.GetQuestById(questID);
            if (quest == null) return;
            journal.AddQuest(quest);
        }

        public void CompleteQuestIfAny()
        {
            if (questID == null) return;
            var journal = FindObjectOfType<Journal>();
            var quest = journal.GetQuestById(questID);
            if (quest == null) return;
            journal.CompleteQuest(quest);
        }

        public void ShowDialog() //set in player controller
        {
            if(dialogueDisplay == null)
                dialogueDisplay = FindObjectOfType<DialogueDisplay>();
            dialogueDisplay.SetActiveVoice(this);
        }

        public IEnumerator LookTowardsPlayer(GameObject player, float dur)
        {
            var targetRotation = Quaternion.LookRotation(player.transform.position - transform.position);
            float t = 0f;
            while(t < dur)
            {
                t += Time.deltaTime;
                float factor = t / dur;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, factor);
                yield return null;
            }

        }
    }

}
