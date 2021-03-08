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
            if (statsTab.activeSelf || abilitiesTab.activeSelf)
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
            if (inventoryTab.activeSelf || abilitiesTab.activeSelf)
            {
                var parentCanvas = GetComponentInParent<Canvas>();
                AbilityTooltip abilityTooltip = parentCanvas.GetComponentInChildren<AbilityTooltip>();
                if (abilityTooltip != null)
                    Destroy(abilityTooltip.gameObject);
                var abilityUI = abilitiesTab.GetComponentInChildren<AbilityUI>();
                abilityUI.DisableAbilitySetter();
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

        public void ToggleSkillsUI()
        {
            if (statsTab.activeSelf || inventoryTab.activeSelf)
            {
                var parentCanvas = GetComponentInParent<Canvas>();
                StatTooltip statTip = parentCanvas.GetComponentInChildren<StatTooltip>();
                if (statTip != null)
                    Destroy(statTip.gameObject);
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
        }

        public void ToggleUI()
        {
            uiContainer.SetActive(!uiContainer.activeSelf);
            if (!uiContainer.activeSelf)
            {
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
    }
}