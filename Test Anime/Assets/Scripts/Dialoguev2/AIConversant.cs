using RPG.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Dialogue2;
public class AIConversant : MonoBehaviour, IRaycastable
{
    public Dialogue dialogue = null;
    [SerializeField] string conversantName;


    public bool HandleRaycast(PlayerController callingController)
    {
        return callingController.InteractWithDialogue();
    }

    public string GetName()
    {
        return conversantName;
    }

    public CursorType GetCursorType()
    {
        return CursorType.Dialogue;
    }
}
