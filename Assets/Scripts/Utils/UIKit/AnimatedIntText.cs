using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class AnimatedIntText : MonoBehaviour, IAnimatingWidget {

	public int Value {
        get { return targetValue; }
        set {
            isSet = true;
            if (!gameObject.activeInHierarchy) {
                HardSetValue(value);
                return;
            }
            if (targetValue == value) { return; }
            if (preventInteractionWhileAnimating) {
                AnimatedWidget.AddAnimating(this);
            }

            transform.DOKill();
            DOTween.To(() => visualValue,
                x => {
                    if (x != visualValue) {
                        PlayChangeSound();
                    }
                    UpdateVisual(x);
                }, value, duration)
                .SetDelay(delay)
                .SetEase(ease)
                .SetTarget(transform)
                .OnComplete(() => {
                    AnimatedWidget.RemoveAnimating(this);
                });

            if (DeltaTextEnabled) {
                if (value > targetValue) {
                    if (animatedIncrement != null) {
                        animatedIncrement.Animate(value - targetValue);
                    }
                } else if (value < targetValue) {
                    if (animatedDecrement != null) {
                        animatedDecrement.Animate(value - targetValue);
                    }
                }
            }

            targetValue = value;
        }
    }

    public string Prefix {
        get { return prefix; }
        set {
            prefix = value;
            UpdateVisual(visualValue);
        }
    }

    public string Postfix {
        get { return postfix; }
        set {
            postfix = value;
            UpdateVisual(visualValue);
        }
    }

    public float Duration { get { return duration; } set { duration = value; } }

    [NonSerialized] public bool DeltaTextEnabled = true;

    [Header("Optional")]
    [SerializeField] private AnimatedIntTextDelta animatedIncrement = default;
    [SerializeField] private AnimatedIntTextDelta animatedDecrement = default;
    [SerializeField] private AudioSource changeSound = default;

    [Header("Values")]
    [SerializeField] private float duration = .2f;
    [SerializeField] private float delay = 0.0f;
    [SerializeField] private Ease ease = Ease.OutSine;
    [Space]
    [SerializeField] private int startValue = 0;
    [SerializeField] private bool preventInteractionWhileAnimating = false;
    [Space]
    [SerializeField] private string prefix;
    [SerializeField] private string postfix;
    [Space]
    [SerializeField] private bool abbreviate = false;
    [SerializeField] private int preferredNumberCount = 4;
    [Space]
    [SerializeField] private float minimalTimeBetweenChangeSound = 0.02f;

    private bool isSet;
    private TextMeshProUGUI text;
    private int visualValue;
    private int targetValue;
    private float timeLastChangeSound;

    public void HardSetValue(int value) {
        isSet = true;
        targetValue = value;
        UpdateVisual(value);
        transform.DOKill();
    }

    private void Awake() {
        if (!isSet) {
            HardSetValue(startValue);
        }
    }

    private void UpdateVisual(int value) {
        if (text == null) {
            text = GetComponent<TextMeshProUGUI>();
        }
        visualValue = value;
        string textValue;
        if (abbreviate) {
            textValue = StringHelper.AbbreviateInt(visualValue, preferredNumberCount);
        } else {
            textValue = visualValue.ToString();
        }
        text.text = prefix + textValue + postfix;
    }

    private void PlayChangeSound() {
        if (Time.time < timeLastChangeSound + minimalTimeBetweenChangeSound) { return; }
        Audio.PlaySound(changeSound);
        timeLastChangeSound = Time.time;
    }

}