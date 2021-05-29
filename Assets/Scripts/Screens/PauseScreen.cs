using UnityEngine;

public class PauseScreen : AbstractScreen<PauseScreen> {

    [SerializeField] private AnimatedButton pauseButton;

    public void Play() {
        Time.timeScale = 1;
    }

    public void Pause() {
        Time.timeScale = 0;
    }

    protected override void OnShow() {
        pauseButton.OnClick += OnPauseButtonClicked;
        gameObject.SetActive(true);
    }

    protected override void OnHide() {
        pauseButton.OnClick -= OnPauseButtonClicked;
        gameObject.SetActive(false);
    }

    private void OnPauseButtonClicked() {

    }

}