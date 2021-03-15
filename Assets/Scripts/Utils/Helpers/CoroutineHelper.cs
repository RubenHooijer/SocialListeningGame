using Oasez.Extensions.Generics.Singleton;
using System.Collections;

public class CoroutineHelper : GenericSingleton<CoroutineHelper, CoroutineHelper> {

    public void StartRoutine(IEnumerator routine) {
        StartCoroutine(routine);
    }

}