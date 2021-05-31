﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInitializer : MonoBehaviour {

    [SerializeField, SceneName] private string startScene;
    [SerializeField] private GameController gameController;

    private void Awake() {
        Application.targetFrameRate = 60;

        StartCoroutine(SceneLoadingRoutine());
    }

    private IEnumerator SceneLoadingRoutine() {
        yield return SceneManager.LoadSceneAsync(startScene, LoadSceneMode.Additive);

        MenuScreen.Instance.Show();
        Instantiate(gameController);

        SceneManager.UnloadSceneAsync("Loader");
    }

}