using RPG.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.CameraUI
{
    public class StatsUI : MonoBehaviour
    {
        [SerializeField] GameObject uiContainer;
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
                uiContainer.SetActive(!uiContainer.activeSelf);
                player.GetComponent<PlayerController>().enabled = !uiContainer.activeSelf;
            }
        }

        public void ToggleUI()
        {
            uiContainer.SetActive(!uiContainer.activeSelf);
            player.GetComponent<PlayerController>().enabled = !uiContainer.activeSelf;
        }
    }
}