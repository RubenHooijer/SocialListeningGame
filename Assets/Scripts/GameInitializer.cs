using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInitializer : MonoBehaviour {

    [SerializeField, SceneName] private string startScene;
    [SerializeField] private GameController gameController;

    private void Awake() {
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        StartCoroutine(SceneLoadingRoutine());
    }

    private IEnumerator SceneLoadingRoutine() {
        yield return SceneManager.LoadSceneAsync(startScene, LoadSceneMode.Additive);

        PasswordScreen.Instance.Show();
        Instantiate(gameController);

        SceneManager.UnloadSceneAsync("Loader");
    }

}