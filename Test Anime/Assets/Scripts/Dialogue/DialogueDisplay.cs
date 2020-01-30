using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Dialogue;
using System;

public class DialogueDisplay : MonoBehaviour
{
    [SerializeField] Text NPCNameField;
    [SerializeField] Text NPCTextField;
    [SerializeField] Transform responseHolder;
    [SerializeField] GameObject responsePrefab;
    GameObject player;
    Voice activeVoice;

    ConversationNode currentNode = null;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        ActivateUI(false);    
    }

    private void Update()
    {

        if(activeVoice != null)
        {
            if (Vector3.Distance(activeVoice.transform.position, player.transform.position) > 5f || Input.GetKeyDown(KeyCode.Escape))
            {
                currentNode = null;
                //activeVoice.gameObject.GetComponent<Animator>().SetBool("isTalking", false);
                activeVoice = null;
                UpdateDisplayForNode(currentNode);
            }
        }

    }

    void ActivateUI(bool activate)
    {
        NPCTextField.transform.parent.gameObject.SetActive(activate);
        responseHolder.gameObject.SetActive(activate);
    }

    public void SetActiveVoice(Voice voice)
    {
        if (activeVoice == voice)
            return;
        activeVoice = voice;
        if (activeVoice != null)
        {
            //activeVoice.gameObject.GetComponent<Animator>().SetBool("isTalking", true);
            activeVoice.StartCoroutine(activeVoice.LookTowardsPlayer(player, .5f));
            NPCNameField.text = activeVoice.GetNPCName();
            var conversation = activeVoice.GetConversation();
            currentNode = conversation.GetRootNode();
        }
        else
            currentNode = null;
        UpdateDisplayForNode(currentNode);
    }

    private void UpdateDisplayForNode(ConversationNode node)
    {
        ActivateUI(node != null);

        SetNPCText(node);

        ClearResponseObjects();

        if(node != null)
        {
            CreateResponsesForNode(node);
        }
    }

    private void SetNPCText(ConversationNode node)
    {
        NPCTextField.text = node != null ? node.text : "";
    }

    private void ClearResponseObjects()
    {
        foreach(Transform child in responseHolder)
        {
            Destroy(child.gameObject);
        }
    }

    private void CreateResponsesForNode(ConversationNode node)
    {
        int choiceIndex = 1;

        foreach(var child in node.children)
        {
            var responseObject = Instantiate(responsePrefab, responseHolder);
            var childNode = activeVoice.GetConversation().GetNodeByUUID(child);
            responseObject.GetComponent<Text>().text = "Choice " + choiceIndex.ToString() + ": " + childNode.text;
            choiceIndex++;
            responseObject.GetComponent<Button>().onClick.AddListener(() => { ChooseResponse(childNode); });
        }
    }

    private void ChooseResponse(ConversationNode childNode)
    {
        if (childNode.actionToTrigger != "")
        {
            activeVoice.TriggerEventForAction(childNode.actionToTrigger);
        }
        if (childNode.children.Count == 0)
        {
            currentNode = null;
            //activeVoice.gameObject.GetComponent<Animator>().SetBool("isTalking", false);
            activeVoice = null;
        }
        else
        {
            currentNode = activeVoice.GetConversation().GetNodeByUUID(childNode.children[0]);
        }
        UpdateDisplayForNode(currentNode);
    }
}
