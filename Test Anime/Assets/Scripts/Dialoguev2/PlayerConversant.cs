using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Dialogue2
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] string playerName;
        Dialogue currentDialogue;
        AIConversant currentConversant = null;
        DialogueNode currentNode = null;
        bool isChoosing = false;

        public event Action onConversationUpdated;

        public bool IsActive()
        {
            return currentDialogue != null;
        }

        public void StartDialogue(AIConversant newConversant, Dialogue newDialogue)
        {
            if (currentDialogue == newDialogue) return;
            currentConversant = newConversant;
            currentDialogue = newDialogue;
            if(currentDialogue != null)
            {
                this.transform.LookAt(newConversant.transform);
                newConversant.transform.LookAt(this.transform);
                currentNode = currentDialogue.GetRootNode();
                onConversationUpdated();
            }
        }

        public void Quit()
        {
            currentDialogue = null;
            currentNode = null;
            isChoosing = false;
            onConversationUpdated();
        }

        // Start is called before the first frame update
        public string GetText()
        {
            if (currentNode == null)
                return "";
            return currentNode.GetText();
        }

        public string GetCurrentConversantName()
        {
            return currentConversant.GetName();
        }

        public bool IsChoosing()
        {
            return isChoosing;
        }

        public IEnumerable<DialogueNode> GetChoices()
        {
            return currentDialogue.GetPlayerChildren(currentNode);
        }

        public void SelectChoice(DialogueNode chosenNode)
        {
            currentNode = chosenNode;
            isChoosing = false;
            Next();
        }

        public void Next()
        {
            int numPlayerResponses = currentDialogue.GetPlayerChildren(currentNode).Count();
            if(numPlayerResponses > 0)
            {
                isChoosing = true;
                onConversationUpdated();
                return;
            }

            DialogueNode[] children = currentDialogue.GetAIChildren(currentNode).ToArray();
            if(children.Length > 0)
            {
                currentNode = children[UnityEngine.Random.Range(0, children.Count())];
                onConversationUpdated();
            }
            else
            {
                Quit();
            }
        }

        public bool HasNext()
        {
            return (currentDialogue.GetAllChildren(currentNode).Count() >= 0);
        }

        //private IEnumerator<DialogueNode> FilterOnCondition(IEnumerable<DialogueNode> inputNode)
        //{
        //    foreach(var node in inputNode)
        //    {
        //        ifnode.
        //    }
        //}
    }
}
