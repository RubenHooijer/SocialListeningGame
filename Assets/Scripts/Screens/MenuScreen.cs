using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScreen : AbstractScreen<MenuScreen> {

    [SerializeField, EventRef] private string backgroundMusic;
    [SerializeField, SceneName] private string sceneToStart;
    [SerializeField] private AnimatedButton startButton;

    protected override void OnShow() {
        startButton.OnClick += OnStartButtonClicked;
        GameController.Instance.FadeMusic(backgroundMusic);

        gameObject.SetActive(true);
    }

    protected override void OnHide() {
        startButton.OnClick -= OnStartButtonClicked;

        gameObject.SetActive(false);
    }

    private void OnStartButtonClicked() {
        SceneManager.sceneLoaded += OnSceneLoaded;
        FadeScreen.Instance.FadeToColor(Color.black, 1, () => {
            SceneManager.LoadSceneAsync(sceneToStart, LoadSceneMode.Additive);
            Hide();
        });
        TimerScreen.gameStartTime = Time.time;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
        SceneManager.SetActiveScene(scene);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}