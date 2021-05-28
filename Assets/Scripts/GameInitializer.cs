using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInitializer : MonoBehaviour {

    [SerializeField, SceneName] private string mainScene;
    [SerializeField, SceneName] private string[] additiveScenes;
    [SerializeField] private GameController gameController;

    private void Awake() {
        Application.targetFrameRate = 60;

        StartCoroutine(SceneLoadingRoutine());
    }

    private IEnumerator SceneLoadingRoutine() {
        for (int i = 0; i < additiveScenes.Length; i++) {
            yield return SceneManager.LoadSceneAsync(additiveScenes[i], LoadSceneMode.Additive);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadSceneAsync(mainScene, LoadSceneMode.Additive);

        SceneManager.UnloadSceneAsync("Loader");
        Instantiate(gameController);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
        SceneManager.SetActiveScene(scene);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}