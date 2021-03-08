using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using RPG.Control;
using UnityEngine.UI;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        SavingSystem savingSystem;
        const string saveFile1 = "save";
        const string saveFile2 = "save2";
        const string saveFile3 = "save3";
        const string autoSaveFile1 = "autosave";
        const string autoSaveFile2 = "autosave2";
        const string autoSaveFile3 = "autosave3";

        [SerializeField] GameObject saveUI;
        bool saveUIOn = false;
        bool isSaving = false;
        [SerializeField] GameObject player;
        [SerializeField] GameObject saveListUI;
        [SerializeField] Text saveFile1Text;
        [SerializeField] Text saveFile2Text;
        [SerializeField] Text saveFile3Text;
        [SerializeField] Text saveLoadText;

        string loadedAutoSaveFile;

        private void Awake()
        {
            savingSystem = GetComponent<SavingSystem>();
            StartCoroutine(LoadLastScene());
        }

        private void Start()
        {
            Debug.Log("Last save file: " + savingSystem.GetLastSaveFile());
            if (!savingSystem.CheckForSave(loadedAutoSaveFile))
            {
                savingSystem.Save(loadedAutoSaveFile);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleSaveUI();
                if (!saveUI.activeInHierarchy)
                    saveListUI.SetActive(false);
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

        IEnumerator LoadTransition(int index)
        {
            ToggleSaveUI();
            Fader fader = FindObjectOfType<Fader>();
            switch (index)
            {
                case 0:
                    if (!savingSystem.CheckForSave(saveFile1))
                        yield break;
                    fader.FadeOutImmediate();
                    yield return GetComponent<SavingSystem>().LoadLastScene(saveFile1);
                    loadedAutoSaveFile = autoSaveFile1;
                    break;
                case 1:
                    if (!savingSystem.CheckForSave(saveFile2))
                        yield break;
                    fader.FadeOutImmediate();
                    yield return GetComponent<SavingSystem>().LoadLastScene(saveFile2);
                    loadedAutoSaveFile = autoSaveFile2;
                    break;
                case 2:
                    if (!savingSystem.CheckForSave(saveFile3))
                        yield break;
                    fader.FadeOutImmediate();
                    yield return GetComponent<SavingSystem>().LoadLastScene(saveFile3);
                    loadedAutoSaveFile = autoSaveFile3;
                    break;
                default:
                    Debug.Log("could not load");
                    break;
            }

            yield return new WaitForSeconds(1);
            yield return fader.FadeIn(2);
        }

        IEnumerator LoadLastScene()
        {
            string lastSave = savingSystem.GetLastSaveFile();
            Debug.Log("last save: " + lastSave);
            yield return savingSystem.LoadLastScene(lastSave);

            switch (lastSave)
            {
                case "save":
                    loadedAutoSaveFile = autoSaveFile1;
                    break;
                case "save2":
                    loadedAutoSaveFile = autoSaveFile2;
                    break;
                case "save3":
                    loadedAutoSaveFile = autoSaveFile3;
                    break;
            }

            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();

            player = GameObject.FindGameObjectWithTag("Player");
            yield return new WaitForSeconds(1);
            yield return fader.FadeIn(2);
        }

        public void SaveButton()
        {
            isSaving = true;
            saveLoadText.text = "Save";
            UpdateSaveFileText();
            saveListUI.SetActive(true);
        }

        public void LoadButton()
        {
            isSaving = false;
            saveLoadText.text = "Load";
            UpdateSaveFileText();
            saveListUI.SetActive(true);
        }

        public void Load(int index)
        {
            //savingSystem.ClearSaveFile(autoSaveFile);
            StartCoroutine(LoadTransition(index));
        }


        public void SaveLoad(int saveIndex)
        {
            SavingSystem saveSystem = GetComponent<SavingSystem>();
            saveListUI.SetActive(false);
            

            if(isSaving)
            {
                string autoToCheck = null;

                switch (saveIndex)
                {
                    case 0:
                        autoToCheck = autoSaveFile1;
                        break;
                    case 1:
                        autoToCheck = autoSaveFile2;
                        break;
                    case 2:
                        autoToCheck = autoSaveFile3;
                        break;
                }

                saveSystem.SwitchAutoToSave(loadedAutoSaveFile, autoToCheck);


                switch (saveIndex)
                {
                    case 0:
                        if (saveSystem.CheckForSave(autoToCheck))
                        {
                            saveSystem.Save(saveFile1);
                            saveSystem.SwitchAutoToSave(autoToCheck, saveFile1);
                        }
                        saveSystem.Save(saveFile1);
                        break;
                    case 1:
                        if (saveSystem.CheckForSave(autoToCheck))
                        {
                            saveSystem.Save(saveFile2);
                            saveSystem.SwitchAutoToSave(autoToCheck, saveFile2);
                        }
                        saveSystem.Save(saveFile2);
                        break;
                    case 2:
                        if (saveSystem.CheckForSave(autoToCheck))
                        {
                            saveSystem.Save(saveFile3);
                            saveSystem.SwitchAutoToSave(autoToCheck, saveFile3);
                        }
                        saveSystem.Save(saveFile3);
                        break;
                    default:
                        Debug.Log("could not save!");
                        break;
                }
            }
            else
                Load(saveIndex);
        }

        public void UpdateSaveFileText()
        {
            saveFile1Text.text = savingSystem.GetSaveWriteTime(saveFile1);
            saveFile2Text.text = savingSystem.GetSaveWriteTime(saveFile2);
            saveFile3Text.text = savingSystem.GetSaveWriteTime(saveFile3);
        }

        public void AutoSave()
        {
            if (saveUIOn)
                ToggleSaveUI();

            savingSystem.Save(loadedAutoSaveFile);
        }

        public void loadAuto()
        {

            savingSystem.Load(loadedAutoSaveFile);
        }
    }
}
