using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool gameHasStarted = false;
    private AsyncOperation asyncLoadLevel;

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
        SceneManager.UnloadSceneAsync(menuSceneName);
        StartCoroutine(AddGameScene());
    }

    IEnumerator AddGameScene()
    {
        asyncLoadLevel = SceneManager.LoadSceneAsync(gameSceneName, LoadSceneMode.Additive);

        while (!asyncLoadLevel.isDone) { yield return null; }

        this.gameHasStarted = true;
    }
}
