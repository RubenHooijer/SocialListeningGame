﻿using System;
using TMPro;
using UnityEngine;

public class TimerScreen : AbstractScreen<TimerScreen> {

    [SerializeField] private float maxPlayTimeInSeconds = 1500;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private AnimatedWidget animatedWidget;

    protected override void OnShow() {
        gameObject.SetActive(true);
    }

    protected override void OnHide() {
        animatedWidget.Hide();

        CoroutineHelper.Delay(animatedWidget.HideDuration + 0.2f, () => gameObject.SetActive(false));
    }

    private void Update() {
        TimeSpan timespan = TimeSpan.FromSeconds(maxPlayTimeInSeconds - Time.time);
        string minuteString = timespan.Minutes.ToString();
        string secondString = timespan.Seconds.ToString();
        minuteString = minuteString.PadLeft(2, '0');
        secondString = secondString.PadLeft(2, '0');
        string timeString = $"{minuteString}:{secondString}";
        timerText.text = timeString;
    }

}