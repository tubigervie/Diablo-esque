using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue2
{
    public class DialogueNode : ScriptableObject
    {
        [SerializeField]
        bool isPlayerSpeaking = false;
        //turn this into an enum if having multiple people speaking
        [SerializeField]
        private string text;
        [SerializeField]
        private List<string> children = new List<string>();
        [SerializeField]
        private Rect rect = new Rect(10, 10, 200, 100); //editor node window
        [SerializeField]
        private List<string> onEnterActions = new List<string>();
        [SerializeField]
        private List<string> onExitActions = new List<string>();

        public string GetText()
        {
            return text;
        }
        public Rect GetRect()
        {
            return rect;
        }

        public List<string> GetChildren()
        {
            return children;
        }

        public bool IsPlayerSpeaking()
        {
            return isPlayerSpeaking;
        }

        public List<string> GetOnEnterActions()
        {
            return onEnterActions;
        }

        public List<string> GetOnExitActions()
        {
            return onExitActions;
        }

#if UNITY_EDITOR
        public void ChangeRect(Vector2 newPosition)
        {
 
            Undo.RecordObject(this, "Move Dialogue Node");
            rect.position = newPosition;
            EditorUtility.SetDirty(this);

        }
        public void ChangeText(string newText)
        {
            if(newText != text)
            {
                Undo.RecordObject(this, "Update Dialogue Text");
                text = newText;
                EditorUtility.SetDirty(this);
            }
        }

        public void RemoveChild(string name)
        {
            Undo.RecordObject(this, "Remove Dialogue Link");
            children.Remove(name);
            EditorUtility.SetDirty(this);
        }

        public void AddChild(string name)
        {
            Undo.RecordObject(this, "Add Dialogue Link");
            children.Add(name);
            EditorUtility.SetDirty(this);
        }

        public void SetPlayerSpeaking(bool flag)
        {
            isPlayerSpeaking = flag;
        }
#endif



    }
}
