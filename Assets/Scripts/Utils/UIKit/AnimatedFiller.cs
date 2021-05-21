using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AnimatedFiller : MonoBehaviour {

    public float Value { get; private set; }

    public float Min { get { return min; } set { min = value; } }
    public float Max { get { return min; } set { min = value; } }

    [Header("References")]
    [SerializeField] private Image fillMask;
    [SerializeField] private AnimatedIntText intText;

    [Header("Settings")]
    [SerializeField] private float duration = .2f;
    [SerializeField] private Ease ease = Ease.InOutSine;
    [Space]
    [SerializeField] private float startValue = 50.0f;
    [SerializeField] private float min = 0.0f;
    [SerializeField] private float max = 100.0f;
    [SerializeField] private bool clamp = true;

    private bool isSet;
    private Tween fillTween;

    public void SetValue(float value, bool instant = false) {
        InitializeIfNeeded();

        if (clamp) {
            value = Mathf.Clamp(value, min, max);
        }

        if (value == Value && !instant) { return; }

        Value = value;

        float targetFill = Mathf.InverseLerp(min, max, value);
        fillTween?.Kill();

        if (instant) {
            fillMask.fillAmount = targetFill;
        } else {
            fillTween = fillMask.DOFillAmount(targetFill, duration)
                .SetEase(ease);
        }

        if (intText != null) {
            int intValue = Mathf.FloorToInt(value);
            if (instant) {
                intText.HardSetValue(intValue);
            } else {
                intText.Value = intValue;
            }
        }
    }

    private void Awake() {
        InitializeIfNeeded();
    }

    private void InitializeIfNeeded() {
        if (isSet) { return; }
        isSet = true;
        SetValue(startValue, true);
    }

}