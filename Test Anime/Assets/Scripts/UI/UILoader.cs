using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UILoader : MonoBehaviour
{

    [SerializeField] string sceneName;

    // Use this for initialization
    void Awake()
    {
        var uiScene = SceneManager.GetSceneByName(sceneName);
        if (!uiScene.isLoaded)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
    }

}
