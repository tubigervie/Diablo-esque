using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Dialogue2;
using UnityEngine.UI;

namespace RPG.UI
{
    public class DialogueUI : MonoBehaviour
    {
        PlayerConversant playerConversant;
        [SerializeField] Text AIText;
        [SerializeField] Text nameText;
        [SerializeField] Button nextButton;
        [SerializeField] Button quitButton;
        [SerializeField] GameObject AIResponse;
        [SerializeField] Transform choiceRoot;
        [SerializeField] GameObject choicePrefab;

        // Start is called before the first frame update
        void Start()
        {
            playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
            playerConversant.onConversationUpdated += UpdateUI;
            nextButton.onClick.AddListener(Next);
            quitButton.onClick.AddListener(Quit);
            UpdateUI();
        }

        void Next()
        {
            playerConversant.Next();
            UpdateUI();
        }

        void Quit()
        {
            playerConversant.Quit();
        }

        void UpdateUI()
        {
            AIResponse.SetActive(playerConversant.IsActive());
            choiceRoot.gameObject.SetActive(playerConversant.IsActive());
            if (!playerConversant.IsActive()) return;
            choiceRoot.gameObject.SetActive(playerConversant.IsChoosing());
            nextButton.gameObject.SetActive(!playerConversant.IsChoosing());
            if (playerConversant.IsChoosing())
            {
                BuildChoiceList();
            }
            else
            {
                AIText.text = playerConversant.GetText();
                nextButton.gameObject.SetActive(playerConversant.HasNext());
            }
        }

        private void BuildChoiceList()
        {
            DetachChildren();
            foreach (DialogueNode choice in playerConversant.GetChoices())
            {
                GameObject choiceInstance = Instantiate(choicePrefab, choiceRoot);
                choiceInstance.GetComponent<Text>().text = choice.GetText();
                Button button = choiceInstance.GetComponent<Button>();
                if(button != null)
                {
                    button.onClick.AddListener(() =>
                    {
                        playerConversant.SelectChoice(choice);
                        UpdateUI();
                    });
                }
            }
        }

        private void DetachChildren()
        {
            foreach (Transform item in choiceRoot)
            {
                Destroy(item.gameObject);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
