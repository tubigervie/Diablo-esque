﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue
{
    [CreateAssetMenu(menuName = ("RPG/Conversation"))]
    public class Conversation : ScriptableObject
    {
        [SerializeField] List<ConversationNode> _nodes = new List<ConversationNode>();
        public delegate void ValidateDelegate();
        public event ValidateDelegate onValidated;

        public List<ConversationNode> nodes
        {
            get
            {
                return _nodes;
            }
        }

        public void AddNode(Vector2 posittion)
        {
            Undo.RecordObject(this, "Add node.");

            var node = new ConversationNode();
            node.position = posittion;
            node.UUID = System.Guid.NewGuid().ToString();
            nodes.Add(node);

            OnValidate();
        }

        public ConversationNode GetNodeByUUID(string UUID)
        {
            foreach(var node in _nodes)
            {
                if (node.UUID == UUID)
                    return node;
            }
            return null;
        }

        public void DeleteNode(ConversationNode node)
        {
            Undo.RecordObject(this, "Delete node.");
            _nodes.Remove(node);
            OnValidate();
        }

        public ConversationNode GetRootNode()
        {
            var rootNodes = new Dictionary<string, ConversationNode>();
            foreach(var node in _nodes)
            {
                rootNodes[node.UUID] = node;
            }

            foreach(var node in _nodes)
            {
                foreach(var child in node.children)
                {
                    if(rootNodes.ContainsKey(child))
                    {
                        rootNodes.Remove(child);
                    }
                }
            }

            foreach(var item in rootNodes)
            {
                return item.Value;
            }

            return null;
        }

        void RemoveNonExistentChildLinks(HashSet<string> UUIDs)
        {
            foreach(var node in _nodes)
            {
                var childrenCopy = node.children.ToArray();
                foreach (var child in childrenCopy)
                {
                    if(!UUIDs.Contains(child))
                    {
                        node.children.Remove(child);
                        EditorUtility.SetDirty(this);
                    }
                }
            }
        }

        private HashSet<string> AssignUUIDs()
        {
            var UUIDs = new HashSet<string>();
            foreach(var node in _nodes)
            {
                while (UUIDs.Contains(node.UUID) || node.UUID == "")
                {
                    node.UUID = System.Guid.NewGuid().ToString();
                    EditorUtility.SetDirty(this);
                }
                UUIDs.Add(node.UUID);
            }
            return UUIDs;
        }

        void OnValidate()
        {
            HashSet<string> UUIDs = AssignUUIDs();

            RemoveNonExistentChildLinks(UUIDs);

            onValidated?.Invoke();
        }
    }
}

