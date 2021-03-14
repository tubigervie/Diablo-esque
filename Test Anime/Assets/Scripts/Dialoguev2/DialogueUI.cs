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
        [SerializeField] Button nextButton;
        [SerializeField] Button quitButton;
        [SerializeField] Transform choiceRoot;
        [SerializeField] GameObject choicePrefab;

        // Start is called before the first frame update
        void Start()
        {
            playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
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
            this.gameObject.SetActive(false);
        }

        void UpdateUI()
        {
            AIText.text = playerConversant.GetText();
            nextButton.gameObject.SetActive(playerConversant.HasNext());
            NewMethod();
        }

        private void NewMethod()
        {
            foreach (Transform item in choiceRoot)
            {
                Destroy(item.gameObject);
            }
            foreach(string choiceText in playerConversant.GetChoices())
            {
                GameObject choiceInstance = Instantiate(choicePrefab, choiceRoot);
                choiceInstance.GetComponent<Text>().text = choiceText;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
