using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreen : AbstractScreen<FadeScreen> {

    [SerializeField] private Image fadeImage;

    public void InstantToBlack() => FadeToBlack(0, null);
    public void FadeToBlack() => FadeToBlack(4, null);
    public void FadeFromBlack() => FadeFromBlack(3, null);

    public void FadeToBlack(float time = 1, Action onDone = null) {
        fadeImage.color = Color.black;
        fadeImage.DOKill();
        fadeImage.DOFade(1, time).From(0)
            .OnComplete(() => onDone?.Invoke());
    }

    public void FadeFromBlack(float time = 1, Action onDone = null) {
        fadeImage.color = Color.black;
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