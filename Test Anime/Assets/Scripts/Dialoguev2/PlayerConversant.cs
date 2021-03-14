using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Dialogue2
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] Dialogue currentDialogue;
        DialogueNode currentNode = null;

        private void Awake()
        {
            currentNode = currentDialogue.GetRootNode();
        }

        // Start is called before the first frame update
        public string GetText()
        {
            if (currentNode == null)
                return "";
            return currentNode.GetText();
        }

        public bool IsChoosing()
        {
            return true;
        }

        public IEnumerable<string> GetChoices()
        {
            yield return "??? I've lived here all my life!";
            yield return "Yeah, I came in from Vada Vada.";
        }

        public void Next()
        {
            DialogueNode[] children = currentDialogue.GetAllChildren(currentNode).ToArray();
            currentNode =  children[Random.Range(0, children.Count())];
        }

        public bool HasNext()
        {
            return (currentDialogue.GetAllChildren(currentNode).Count() >= 0);
        }
    }
}
