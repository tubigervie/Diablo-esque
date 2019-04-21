using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Saving
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";

        private void Start()
        {
            Load();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Save();
            else if (Input.GetKeyDown(KeyCode.L))
            {
                StartCoroutine("LoadTransition");
            }
        }

        IEnumerator LoadTransition()
        {
            SceneManagement.Fader fader = FindObjectOfType<SceneManagement.Fader>();
            yield return fader.FadeOut(1);
            Load();
            yield return fader.FadeIn(1);
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }
    }
}
