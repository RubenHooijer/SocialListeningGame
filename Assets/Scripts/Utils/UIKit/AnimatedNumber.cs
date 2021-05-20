using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AnimatedNumber : MonoBehaviour {

    [SerializeField] private Text text;

    private Coroutine routine;

    public void Set(int value) {
        text.text = value.ToString();
    }

    public void Animate(int from, int to, float duration, EasingType ease, Action onDone = null) {
        if (routine != null) {
            StopCoroutine(routine);
        }
        routine = StartCoroutine(AnimateOverTime(from, to, duration, ease, onDone));
    }

    private IEnumerator AnimateOverTime(int from, int to, float duration, EasingType ease, Action onDone = null) {
        text.text = from.ToString();

        float time = 0.0f;

        while (time < duration) {
            time += Time.deltaTime;

            float progress = Mathf.Min(time / duration, 1.0f);
            float easedProgress = EasingHelper.Ease(ease, progress);
            float value = Mathf.Lerp(from, to, easedProgress);
            int intValue = Mathf.CeilToInt(value);

            text.text = intValue.ToString();

            yield return new WaitForEndOfFrame();
        }

        text.text = to.ToString();

        if (onDone != null) {
            onDone();
        }
    }

}
