using Oasez.Extensions.Generics.Singleton;
using System;
using System.Collections;
using UnityEngine;

public class CoroutineHelper : GenericSingleton<CoroutineHelper, CoroutineHelper> {

    public static Coroutine Delay(float duration, Action onDelayed, bool unscaled = true, bool blockInteractions = false) {
        if (duration <= 0) {
            onDelayed?.Invoke();
            return null;
        }
        return Instance.StartCoroutine(DelayOverTime(duration, onDelayed, unscaled, blockInteractions));
    }

    public static Coroutine Delay(Func<bool> predicate, Action onTrue) {
        return Instance.StartCoroutine(DelayTillTrue(predicate, onTrue));
    }

    public static Coroutine DelayFrame(Action onDelayed) {
        return Instance.StartCoroutine(DelayOverFrames(1, onDelayed));
    }

    public static Coroutine Start(IEnumerator routine) {
        return Instance.StartLocalCoroutine(routine);
    }

    public static void Stop(Coroutine routine) {
        if (routine == null) { return; }
        Instance.StopCoroutine(routine);
    }

    private Coroutine StartLocalCoroutine(IEnumerator routine) {
        return StartCoroutine(routine);
    }

    private static IEnumerator DelayOverTime(float duration, Action onDelayed, bool unscaled, bool blockInteractions = false) {
        string blockInteractionId = Guid.NewGuid().ToString();

        if (blockInteractions) {
            AnimatedWidget.AddAnimating(blockInteractionId);
        }

        if (unscaled) {
            yield return new WaitForSecondsRealtime(duration);
        } else {
            yield return new WaitForSeconds(duration);
        }

        if (blockInteractions) {
            AnimatedWidget.RemoveAnimating(blockInteractionId);
        }

        onDelayed?.Invoke();
    }

    private static IEnumerator DelayOverFrames(int frames, Action onDelayed) {
        for (int i = 0; i < frames; i++) {
            yield return null;
        }
        onDelayed?.Invoke();
    }

    private static IEnumerator DelayTillTrue(Func<bool> predicate, Action onTrue) {
        yield return new WaitUntil(predicate);
        onTrue?.Invoke();
    }

}