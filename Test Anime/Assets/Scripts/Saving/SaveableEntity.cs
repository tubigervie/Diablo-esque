using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace RPG.Saving
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] string uniqueIdentifier = "";
        public string GetUniqueIdentifier()
        {
            return "";
        }

        public object CaptureState()
        {
            Debug.Log("Capturing state for: " + GetUniqueIdentifier());
            return null;
        }

        public void RestoreState(object state)
        {
            Debug.Log("Restoring state for: " + GetUniqueIdentifier());
        }

        private void Update()
        {
            if (Application.IsPlaying(this.gameObject))
                return;
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");
            if (gameObject.scene.path == "")
                return;
            if (property.stringValue == "")
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}

