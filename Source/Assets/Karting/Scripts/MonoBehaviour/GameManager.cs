using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private AsyncOperation asyncLoadLevel;
    private AsyncOperation asyncUnloadLevel;

    public string gameSceneName;
    public string menuSceneName;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetSceneByName(gameSceneName).isLoaded)
        {
            SceneManager.UnloadSceneAsync(gameSceneName);
        }

        if (!SceneManager.GetSceneByName(menuSceneName).isLoaded)
        {
            SceneManager.LoadSceneAsync(menuSceneName, LoadSceneMode.Additive);
        }
    }

    internal void StartGame()
    {
        asyncUnloadLevel = SceneManager.UnloadSceneAsync(menuSceneName);
        StartCoroutine(AddGameScene());
    }

    IEnumerator AddGameScene()
    {
        asyncLoadLevel = SceneManager.LoadSceneAsync(gameSceneName, LoadSceneMode.Additive);

        while (!asyncLoadLevel.isDone) { yield return null; }
    }

    internal void PlayAgain()
    {
        if (SceneManager.GetSceneByName(menuSceneName).isLoaded)
        {
            SceneManager.UnloadSceneAsync(menuSceneName);
        }

        if (SceneManager.GetSceneByName(gameSceneName).isLoaded)
        {
            asyncUnloadLevel = SceneManager.UnloadSceneAsync(gameSceneName);
        }

        StartCoroutine(AddGameScene());
    }

    internal void BackToMainMenu()
    {
        if (SceneManager.GetSceneByName(gameSceneName).isLoaded)
        {
            SceneManager.UnloadSceneAsync(gameSceneName);
        }

        if (!SceneManager.GetSceneByName(menuSceneName).isLoaded)
        {
            SceneManager.LoadSceneAsync(menuSceneName, LoadSceneMode.Additive);
        }
    }
}
