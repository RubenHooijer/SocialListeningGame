using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreen : AbstractScreen<FadeScreen> {

    private const string START_OVER_TEXT0 = "<color=white>Opnieuw.</color><color=black> Herrijs.\nBevrijd de luistermutant.</color>";
    private const string START_OVER_TEXT1 = "<color=white>Opnieuw. Herrijs.</color><color=black>\nBevrijd de luistermutant.</color>";
    private const string START_OVER_TEXT2 = "<color=white>Opnieuw. Herrijs.\nBevrijd de luistermutant.</color>";

    [SerializeField] private VoidEventChannelSO FadeToCompleted;
    [SerializeField] private VoidEventChannelSO FadeFromCompleted;
    [SerializeField] private Image fadeImage;
    [SerializeField] private TextMeshProUGUI startOverText; 

    public void InstantToBlack() => FadeToColor(Color.black, 0, null);
    public void FadeToBlack() => FadeToColor(Color.black, 4, null);
    public void FadeFromBlack() => FadeFromColor(Color.black, 3, null);
    public void FadeToWhite() => FadeToColor(Color.white, 2, null);
    public void FadeFromWhite() => FadeFromColor(Color.white, 3, null);

    public void FadeWithText() {
        startOverText.text = string.Empty;
        FadeToColor(Color.black, 1, () => {
            CoroutineHelper.Delay(1.5f, () => startOverText.text = START_OVER_TEXT0);
            CoroutineHelper.Delay(3f, () => startOverText.text = START_OVER_TEXT1);
            CoroutineHelper.Delay(4.7f, () => startOverText.text = START_OVER_TEXT2);
            CoroutineHelper.Delay(5.3f, () => startOverText.transform.DOPunchScale(Vector3.one * 0.1f, 0.3f));
            CoroutineHelper.Delay(5.7f, () => startOverText.DOFade(0, 0.5f));
            CoroutineHelper.Delay(7f, FadeToCompleted.Raise);
        });
    }

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