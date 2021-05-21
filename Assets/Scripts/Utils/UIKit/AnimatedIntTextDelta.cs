using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class AnimatedIntTextDelta : MonoBehaviour, IAnimatingWidget {

    [Header("References")]
    [SerializeField] private TextMeshProUGUI text;

    [Header("Values")]
    [SerializeField] private bool preventInteractionWhileAnimating;

    [Header("Animation")]
    [SerializeField] private Vector2 endOffset = Vector2.down * 50.0f;
    [SerializeField] private float incrementDuration = .5f;
    [SerializeField] private float decrementDuration = .5f;

    public void Animate(int amount, Action onDone = null) {
        text.text = amount.ToString();

        float duration = decrementDuration;
        if (amount > 0) {
            text.text = "+" + amount.ToString();
            duration = incrementDuration;
        }

        if (preventInteractionWhileAnimating) {
            AnimatedWidget.AddAnimating(this);
        }

        text.DOKill();
        text.alpha = 1.0f;
        text.DOFade(.0f, duration);

        text.transform.DOKill();
        text.transform.localPosition = Vector2.zero;
        text.transform.DOLocalMove(endOffset, duration)
            .SetEase(Ease.OutSine)
            .OnComplete(() => {
                AnimatedWidget.RemoveAnimating(this);
                if (onDone != null) {
                    onDone();
                }
            });
    }

    private void Start() {
        text.alpha = .0f;
    }

}