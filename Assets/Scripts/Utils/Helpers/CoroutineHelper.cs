using Oasez.Extensions.Generics.Singleton;
using System;
using System.Collections;
using UnityEngine;

public class CoroutineHelper : GenericSingleton<CoroutineHelper, CoroutineHelper> {

    public void StartCoroutine(Action action, float afterTime) {
        StartCoroutine(WaitCoroutine(action, afterTime));
    }

    public void StartCoroutine(Action action, Func<bool> condition) {
        StartCoroutine(ConditionCoroutine(action, condition));
    }

    public IEnumerator WaitCoroutine(Action action, float afterTime) {
        yield return new WaitForSeconds(afterTime);
        action.Invoke();
    }

    public IEnumerator ConditionCoroutine(Action action, Func<bool> condition) {
        yield return new WaitUntil(condition);
        action.Invoke();
    }

}