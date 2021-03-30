using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInitializer : MonoBehaviour {

    [SerializeField, SceneName] private string[] startingScenes;

    private void Awake() {
        StartCoroutine(SceneLoadingRoutine());
    }

    private IEnumerator SceneLoadingRoutine() {
        for (int i = 0; i < startingScenes.Length; i++) {
            yield return SceneManager.LoadSceneAsync(startingScenes[i], LoadSceneMode.Additive);
        }

        SceneManager.UnloadSceneAsync("Loader");
    }

}