using System;
using TMPro;
using UnityEngine;

public class TimerScreen : AbstractScreen<TimerScreen> {

    [SerializeField] private float maxPlayTimeInSeconds = 1500;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private AnimatedWidget animatedWidget;

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
        float timeLeft = maxPlayTimeInSeconds - Time.time;

        if (timeLeft <= 10) {
            animatedWidget.Show();
        }

        if (animatedWidget.IsShowing) { 
            UpdateText(timeLeft);
        }
    }

    private void UpdateText(float timeLeft) {
        TimeSpan timespan = TimeSpan.FromSeconds(timeLeft);
        string minuteString = timespan.Minutes.ToString();
        string secondString = timespan.Seconds.ToString();
        minuteString = minuteString.PadLeft(2, '0');
        secondString = secondString.PadLeft(2, '0');
        string timeString = $"{minuteString}:{secondString}";
        timerText.text = timeString;
    }

}