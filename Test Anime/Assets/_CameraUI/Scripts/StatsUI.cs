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

        GameObject player;
        // Start is called before the first frame update
        void Start()
        {
            uiContainer.SetActive(false);
            player = GameObject.FindGameObjectWithTag("Player");
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                DisableAllTabs();
                inventoryTab.SetActive(true);
                uiContainer.SetActive(!uiContainer.activeSelf);
                if(!uiContainer.activeSelf)
                {
                    invUI.DisableActiveButton();
                }
                player.GetComponent<PlayerController>().enabled = !uiContainer.activeSelf;
            }
            if(Input.GetKeyDown(KeyCode.C))
            {
                DisableAllTabs();
                statsTab.SetActive(true);
                uiContainer.SetActive(!uiContainer.activeSelf);
                player.GetComponent<PlayerController>().enabled = !uiContainer.activeSelf;
            }
        }

        void DisableAllTabs()
        {
            inventoryTab.SetActive(false);
            statsTab.SetActive(false);
        }

        public void ToggleUI()
        {
            uiContainer.SetActive(!uiContainer.activeSelf);
            if (!uiContainer.activeSelf)
            {
                invUI.DisableActiveButton();
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
    }
}