using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue2
{
    [CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue", order = 0)]
    public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] List<DialogueNode> nodes = new List<DialogueNode>();
        [SerializeField] Vector2 newNodeOffset = new Vector2(250, 0);
        Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();


        private void OnValidate()
        {
            nodeLookup.Clear();
            foreach(DialogueNode node in GetAllNodes())
            {
                nodeLookup[node.name] = node;
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
            foreach(string childID in parentNode.GetChildren())
            {
                if (nodeLookup.ContainsKey(childID))
                {
                    yield return nodeLookup[childID];
                }
            }
        }

#if UNITY_EDITOR  //preprocessor directiv
        public void CreateNode(DialogueNode parent)
        {
            DialogueNode newChild = MakeNode(parent);
            Undo.RegisterCreatedObjectUndo(newChild, "Created Dialogue Node");
            Undo.RecordObject(this, "Added Dialogue Node");
            AddNode(newChild);
        }

        private void AddNode(DialogueNode newChild)
        {
            nodes.Add(newChild);
            OnValidate();
        }

        private DialogueNode MakeNode(DialogueNode parent)
        {
            DialogueNode newChild = ScriptableObject.CreateInstance<DialogueNode>();
            newChild.name = Guid.NewGuid().ToString();
            if (parent != null)
            {
                parent.AddChild(newChild.name);
                if (parent.IsPlayerSpeaking())
                    newChild.SetPlayerSpeaking(false);
                else
                    newChild.SetPlayerSpeaking(true);
                newChild.ChangeRect(parent.GetRect().position + newNodeOffset);
            }
            return newChild;
        }

        public void DeleteNode(DialogueNode nodeToDelete)
        {
            Undo.RecordObject(this, "Deleted Dialogue Node");
            nodes.Remove(nodeToDelete);
            OnValidate();
            CleanDanglingChildren(nodeToDelete);
            Undo.DestroyObjectImmediate(nodeToDelete);
        }

        private void CleanDanglingChildren(DialogueNode nodeToDelete)
        {
            foreach (DialogueNode node in GetAllNodes())
            {
                node.RemoveChild(nodeToDelete.name);
            }
        }

        public void OnBeforeSerialize()
        {
            if (nodes.Count == 0)
            {
                DialogueNode newChild = MakeNode(null);
                AddNode(newChild);
            }

            string assetPath = AssetDatabase.GetAssetPath(this);
            if (!String.IsNullOrEmpty(assetPath))
            {
                foreach(DialogueNode node in GetAllNodes())
                {
                    if(AssetDatabase.GetAssetPath(node) == "")
                        AssetDatabase.AddObjectToAsset(node, this);
                }
            }
#endif 
        }

        public void OnAfterDeserialize()
        {
        }
    }
}

