using RPG.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.CameraUI
{
    public class StatsUI : MonoBehaviour
    {
        [SerializeField] GameObject uiContainer;
        [SerializeField] InventoryUI invUI;
        [SerializeField] GameObject inventoryTab;
        [SerializeField] GameObject statsTab;
        [SerializeField] GameObject abilitiesTab;
        [SerializeField] GameObject questsTab;

        GameObject player;
        // Start is called before the first frame update
        void Start()
        {
            uiContainer.SetActive(false);
            player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<PlayerController>().SetStatsUI(this);
            DisableAllTabs();
        }

        public void ToggleInventoryUI()
        {
            if (statsTab.activeSelf || abilitiesTab.activeSelf || questsTab.activeSelf)
            {
                var parentCanvas = GetComponentInParent<Canvas>();
                StatTooltip statTip = parentCanvas.GetComponentInChildren<StatTooltip>();
                if (statTip != null)
                    Destroy(statTip.gameObject);
                AbilityTooltip abilityTooltip = parentCanvas.GetComponentInChildren<AbilityTooltip>();
                if (abilityTooltip != null)
                    Destroy(abilityTooltip.gameObject);
                var abilityUI = abilitiesTab.GetComponentInChildren<AbilityUI>();
                abilityUI.DisableAbilitySetter();
                QuestDetailsUI questDetails = FindObjectOfType<QuestDetailsUI>();
                if (questDetails != null)
                    questDetails.gameObject.SetActive(false);
                SwitchToInventory();
                return;
            }
            DisableAllTabs();
            inventoryTab.SetActive(!uiContainer.activeSelf);
            uiContainer.SetActive(!uiContainer.activeSelf);
            if (!uiContainer.activeSelf)
            {
                invUI.DisableActiveButton();
            }
            //player.GetComponent<PlayerController>().enabled = !uiContainer.activeSelf;
        }

        public void ToggleCharacterStatsUI()
        {
            if (inventoryTab.activeSelf || abilitiesTab.activeSelf || questsTab.activeSelf)
            {
                var parentCanvas = GetComponentInParent<Canvas>();
                AbilityTooltip abilityTooltip = parentCanvas.GetComponentInChildren<AbilityTooltip>();
                if (abilityTooltip != null)
                    Destroy(abilityTooltip.gameObject);
                var abilityUI = abilitiesTab.GetComponentInChildren<AbilityUI>();
                abilityUI.DisableAbilitySetter();
                QuestDetailsUI questDetails = FindObjectOfType<QuestDetailsUI>();
                if (questDetails != null)
                    questDetails.ToggleOff();
                SwitchToStats();
                return;
            }
            DisableAllTabs();
            statsTab.SetActive(!uiContainer.activeSelf);
            uiContainer.SetActive(!uiContainer.activeSelf);
            if (!uiContainer.activeSelf)
            {
                invUI.DisableActiveButton();
                var parentCanvas = GetComponentInParent<Canvas>();
                StatTooltip statTip = parentCanvas.GetComponentInChildren<StatTooltip>();
                if (statTip != null)
                    Destroy(statTip.gameObject);
            }
            //player.GetComponent<PlayerController>().enabled = !uiContainer.activeSelf;
        }

        public void ToggleQuestsUI()
        {
            if (inventoryTab.activeSelf || abilitiesTab.activeSelf || statsTab.activeSelf)
            {
                var parentCanvas = GetComponentInParent<Canvas>();
                AbilityTooltip abilityTooltip = parentCanvas.GetComponentInChildren<AbilityTooltip>();
                if (abilityTooltip != null)
                    Destroy(abilityTooltip.gameObject);
                var abilityUI = abilitiesTab.GetComponentInChildren<AbilityUI>();
                abilityUI.DisableAbilitySetter();
                SwitchToQuests();
                return;
            }
            QuestDetailsUI questDetails = FindObjectOfType<QuestDetailsUI>();
            if (questDetails != null)
                questDetails.ToggleOff();
            DisableAllTabs();
            questsTab.SetActive(!uiContainer.activeSelf);
            uiContainer.SetActive(!uiContainer.activeSelf);
            if (!uiContainer.activeSelf)
            {
                Debug.Log("being turned off");
                invUI.DisableActiveButton();
                var parentCanvas = GetComponentInParent<Canvas>();
                StatTooltip statTip = parentCanvas.GetComponentInChildren<StatTooltip>();
                if (statTip != null)
                    Destroy(statTip.gameObject);
            }
            //player.GetComponent<PlayerController>().enabled = !uiContainer.activeSelf;
        }

        public void ToggleSkillsUI()
        {
            if (statsTab.activeSelf || inventoryTab.activeSelf || questsTab.activeSelf)
            {
                var parentCanvas = GetComponentInParent<Canvas>();
                StatTooltip statTip = parentCanvas.GetComponentInChildren<StatTooltip>();
                if (statTip != null)
                    Destroy(statTip.gameObject);
                QuestDetailsUI questDetails = FindObjectOfType<QuestDetailsUI>();
                if (questDetails != null)
                    questDetails.ToggleOff();
                SwitchToAbilities();
                return;
            }
            DisableAllTabs();
            abilitiesTab.SetActive(!uiContainer.activeSelf);
            uiContainer.SetActive(!uiContainer.activeSelf);
            if (!uiContainer.activeSelf)
            {
                invUI.DisableActiveButton();
                var parentCanvas = GetComponentInParent<Canvas>();
                AbilityTooltip abilityTooltip = parentCanvas.GetComponentInChildren<AbilityTooltip>();
                if (abilityTooltip != null)
                    Destroy(abilityTooltip.gameObject);
                var abilityUI = abilitiesTab.GetComponentInChildren<AbilityUI>();
                abilityUI.DisableAbilitySetter();
            }
            //player.GetComponent<PlayerController>().enabled = !uiContainer.activeSelf;
        }

        // Update is called once per frame
        void Update()
        {
        }

        void DisableAllTabs()
        {
            inventoryTab.SetActive(false);
            statsTab.SetActive(false);
            abilitiesTab.SetActive(false);
            questsTab.SetActive(false);
        }

        public void ToggleUI()
        {
            uiContainer.SetActive(!uiContainer.activeSelf);
            if (!uiContainer.activeSelf)
            {
                QuestDetailsUI questDetails = FindObjectOfType<QuestDetailsUI>();
                if (questDetails != null)
                    questDetails.ToggleOff();
                DisableAllTabs();
                invUI.DisableActiveButton();
                var parentCanvas = GetComponentInParent<Canvas>();
                StatTooltip statTip = parentCanvas.GetComponentInChildren<StatTooltip>();
                if (statTip != null)
                    Destroy(statTip.gameObject);
                AbilityTooltip abilityTooltip = parentCanvas.GetComponentInChildren<AbilityTooltip>();
                if (abilityTooltip != null)
                    Destroy(abilityTooltip.gameObject);
                var abilityUI = abilitiesTab.GetComponentInChildren<AbilityUI>();
                abilityUI.DisableAbilitySetter();
            }
            player.GetComponent<PlayerController>().enabled = !uiContainer.activeSelf;
        }

        public void SwitchToStats()
        {
            DisableAllTabs();
            statsTab.SetActive(true);
        }

        public void SwitchToInventory()
        {
            DisableAllTabs();
            inventoryTab.SetActive(true);
        }

        public void SwitchToAbilities()
        {
            DisableAllTabs();
            abilitiesTab.SetActive(true);
        }

        public void SwitchToQuests()
        {
            DisableAllTabs();
            questsTab.SetActive(true);
        }
    }
}