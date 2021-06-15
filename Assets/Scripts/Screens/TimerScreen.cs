using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerScreen : AbstractScreen<TimerScreen> {

    public static float gameStartTime = 0;

    [SerializeField, SceneName] private string failScene;
    [SerializeField] private float maxPlayTimeInSeconds = 1500;
    [SerializeField] private float showTimerThreshold = 60;
    [SerializeField] private ProgressionKey gameFinishedKey;

    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private AnimatedWidget animatedWidget;

    private bool isFinished = false;

    public void ShowShortTime() {
        if (animatedWidget.IsShowing) { return; }
        Show();
        CoroutineHelper.Delay(10, () => {
            Hide();
        });
    }

    protected override void Awake() {
        base.Awake();
        gameObject.SetActive(true);
        Hide();
    }

    protected override void OnShow() {
        animatedWidget.Show();
    }

    protected override void OnHide() {
        animatedWidget.Hide();
    }

    private void Update() {
        if (gameStartTime == 0 || World.Instance.Progression.HasKey(gameFinishedKey)) { return; }

        float timeLeft = (maxPlayTimeInSeconds + gameStartTime) - Time.time;

        if (!isFinished && 
            timeLeft <= showTimerThreshold && 
            !animatedWidget.IsShowing) {
            animatedWidget.Show();
        }

        if (animatedWidget.IsShowing) { 
            UpdateText(timeLeft);
        }

        if (!isFinished && timeLeft <= 0) {
            OnTimerFinished();
        }
    }

    private void OnTimerFinished() {
        isFinished = true;
        Hide();
        if (World.Instance.Progression.HasKey(gameFinishedKey)) { return; }

        FadeScreen.Instance.FadeToColor(Color.black, 2, () => {
            FindObjectsOfType<BaseScreen>().Foreach(x => x.Hide());
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadSceneAsync(failScene, LoadSceneMode.Additive);
        });
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
        SceneManager.SetActiveScene(scene);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void UpdateText(float timeLeft) {
        if (timeLeft <= 0) {
            timerText.text = "00:00";
            return;
        }

        TimeSpan timespan = TimeSpan.FromSeconds(timeLeft);
        string minuteString = timespan.Minutes.ToString();
        string secondString = timespan.Seconds.ToString();
        minuteString = minuteString.PadLeft(2, '0');
        secondString = secondString.PadLeft(2, '0');
        string timeString = $"{minuteString}:{secondString}";
        timerText.text = timeString;
    }

}