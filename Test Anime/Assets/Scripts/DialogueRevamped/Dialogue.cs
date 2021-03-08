using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue2
{
    [CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue", order = 0)]
    public class Dialogue : ScriptableObject
    {
        [SerializeField] List<DialogueNode> nodes = new List<DialogueNode>();

        Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();

#if UNITY_EDITOR //preprocessor directive
        private void Awake() //for SOs, awake is triggered when ever the scriptable object is loaded (i.e. when a scriptable object is created)
        {
            if(nodes.Count == 0)
            {
                DialogueNode rootNode = new DialogueNode();
                rootNode.uniqueID = Guid.NewGuid().ToString();
                nodes.Add(rootNode);
            }
            OnValidate();
        }
#endif 

        private void OnValidate()
        {
            nodeLookup.Clear();
            foreach(DialogueNode node in GetAllNodes())
            {
                nodeLookup[node.uniqueID] = node;
            }
        }

        public IEnumerable<DialogueNode> GetAllNodes() //IEnumerable is a general type for collections that can be iterated over (either a list or an array). Allows you to have a general type
        {
            return nodes;
        }

        public DialogueNode GetRootNode()
        {
            return nodes[0];
        }

        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
        {
            foreach(string childID in parentNode.children)
            {
                if (nodeLookup.ContainsKey(childID))
                {
                    yield return nodeLookup[childID];
                }
            }
        }

        public void CreateNode(DialogueNode parent)
        {
            DialogueNode newChild = new DialogueNode();
            newChild.uniqueID = Guid.NewGuid().ToString();
            parent.children.Add(newChild.uniqueID);
            nodes.Add(newChild);
            OnValidate();
        }

        public void DeleteNode(DialogueNode nodeToDelete)
        {
            nodes.Remove(nodeToDelete);
            OnValidate();
            CleanDanglingChildren(nodeToDelete);
        }

        private void CleanDanglingChildren(DialogueNode nodeToDelete)
        {
            foreach (DialogueNode node in GetAllNodes())
            {
                node.children.Remove(nodeToDelete.uniqueID);
            }
        }
    }
}

