using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreen : AbstractScreen<FadeScreen> {

    [SerializeField] private VoidEventChannelSO FadeToCompleted;
    [SerializeField] private VoidEventChannelSO FadeFromCompleted;
    [SerializeField] private Image fadeImage;

    public void InstantToBlack() => FadeToColor(Color.black, 0, null);
    public void FadeToBlack() => FadeToColor(Color.black, 4, null);
    public void FadeFromBlack() => FadeFromColor(Color.black, 3, null);
    public void FadeToWhite() => FadeToColor(Color.white, 2, null);
    public void FadeFromWhite() => FadeFromColor(Color.white, 3, null);

    public void FadeToColor(Color toColor, float time = 1, Action onDone = null) {
        onDone += FadeToCompleted.Raise;

        fadeImage.color = toColor;
        fadeImage.DOKill();
        fadeImage.DOFade(1, time).From(0)
            .OnComplete(() => onDone?.Invoke());
    }

    public void FadeFromColor(Color fromColor, float time = 1, Action onDone = null) {
        onDone += FadeFromCompleted.Raise;

        fadeImage.color = fromColor;
        fadeImage.DOKill();
        fadeImage.DOFade(0, time).From(1)
            .OnComplete(() => onDone?.Invoke());
    }

    protected override void Awake() {
        base.Awake();

        gameObject.SetActive(true);
        fadeImage.color = Color.clear;
    }

    protected override void OnShow() { }
    protected override void OnHide() { }

}