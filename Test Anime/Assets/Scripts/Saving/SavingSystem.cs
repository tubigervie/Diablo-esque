using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public void Save(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);
            CaptureState(state);
            SaveFile(saveFile, state);
        }

        private void SaveFile(string saveFile, object state)
        {
            string path = GetPathFromSaveFile(saveFile);
            print("Saving to " + path);
            using (FileStream stream = File.Open(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, state);
            }
        }

        public void ClearSaveFile(string saveFile)
        {
            string savePath = GetPathFromSaveFile(saveFile);
            if (File.Exists(savePath))
                File.Delete(savePath);
        }

        private void CaptureState(Dictionary<string, object> state)
        {
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                state[saveable.GetUniqueIdentifier()] = saveable.CaptureState();
            }
            state["currentSceneBuildIndex"] = SceneManager.GetActiveScene().name;
        }

        public string[] GetSaveFileList()
        {
            var filePaths = new string[3] { "save", "save2", "save3" };
            return filePaths;
        }

        public string GetLastSaveFile()
        {
            var saveFiles = GetSaveFileList();
            string lastSaveFile = null;
            DateTime lastSaveFileWriteTime = DateTime.MinValue;
            foreach(var saveFile in saveFiles)
            {
                var writeTime = File.GetLastWriteTime(GetPathFromSaveFile(saveFile));
                if(writeTime > lastSaveFileWriteTime)
                {
                    lastSaveFileWriteTime = writeTime;
                    lastSaveFile = saveFile;
                }
            }
            return lastSaveFile;
        }

        public string GetSaveWriteTime(string saveFile)
        {
            if(CheckForSave(saveFile))
            {
                var writeTime = File.GetLastWriteTime(GetPathFromSaveFile(saveFile));
                return writeTime.ToString("yyyy-MM-dd HH.mm.ss");
            }
            return "";
        }

        public void Load(string saveFile)
        {     
            RestoreState(LoadFile(saveFile));
        }

        public IEnumerator LoadLastScene(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);
            string buildIndex = SceneManager.GetActiveScene().name;
            if (state.ContainsKey("currentSceneBuildIndex"))
            {
                buildIndex = (string)state["currentSceneBuildIndex"];
            }
            yield return SceneManager.LoadSceneAsync(buildIndex);
            RestoreState(state);
        }

        private Dictionary<string, object> LoadFile(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            if(!File.Exists(path))
            {
                return new Dictionary<string, object>();
            }
            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (Dictionary<string, object>) formatter.Deserialize(stream);
            }
        }

        private void RestoreState(Dictionary<string, object> state)
        {
            foreach(SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                string id = saveable.GetUniqueIdentifier();
                if (state.ContainsKey(id))
                {
                    saveable.RestoreState(state[id]);
                }
            }
        }

        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }

        public bool CheckForSave(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            return File.Exists(path);
        }

        public void SwitchAutoToSave(string autoFile, string saveFile)
        {
            Dictionary<string, object> state;
            using (FileStream stream = File.Open(GetPathFromSaveFile(autoFile), FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                state = (Dictionary<string, object>)formatter.Deserialize(stream);
            }
            using (FileStream stream = File.Open(GetPathFromSaveFile(saveFile), FileMode.Create))
            {
                BinaryFormatter newFormatter = new BinaryFormatter();
                newFormatter.Serialize(stream, state);
            }
        }

        public void DeleteAutoSave(string saveFile)
        {
            File.Delete(GetPathFromSaveFile(saveFile));
        }
    }   

}

