using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Saving
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                GetComponent<SavingSystem>().Save(defaultSaveFile);
            else if (Input.GetKeyDown(KeyCode.L))
                GetComponent<SavingSystem>().Load(defaultSaveFile);
        }
    }
}
