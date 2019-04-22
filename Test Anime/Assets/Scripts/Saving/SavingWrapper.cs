using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using RPG.Control;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        SavingSystem savingSystem;
        const string defaultSaveFile = "save";
        const string autoSaveFile = "autosave";
        [SerializeField] GameObject saveUI;
        bool saveUIOn = false;

        [SerializeField] GameObject player;



        private IEnumerator Start()
        {
            
            GetComponent<SavingSystem>().DeleteAutoSave(autoSaveFile);
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            player = GameObject.FindGameObjectWithTag("Player");
            yield return new WaitForSeconds(1);
            yield return fader.FadeIn(2);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleSaveUI();
            }
        }

        private void ToggleSaveUI()
        {
            if(player == null)
                player = GameObject.FindGameObjectWithTag("Player");
            saveUIOn = !saveUIOn;
            saveUI.SetActive(saveUIOn);
            player.GetComponent<PlayerController>().enabled = !saveUIOn;
        }

        IEnumerator LoadTransition()
        {
            ToggleSaveUI();
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            yield return new WaitForSeconds(1);
            yield return fader.FadeIn(2);
        }

        public void Load()
        {
            StartCoroutine("LoadTransition");
        }

        public void Save()
        {
            ToggleSaveUI();
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        public void AutoSave()
        {
            if (saveUIOn)
                ToggleSaveUI();
            GetComponent<SavingSystem>().Save(autoSaveFile);
        }

        public void loadAuto()
        {
            GetComponent<SavingSystem>().Load(autoSaveFile);
        }
    }
}
