using UnityEngine.Events;

public static class Eventbus {

    public readonly static UnityEvent<ProgressionKey, bool> ProgressionKeyChangedEvent = new UnityEvent<ProgressionKey, bool>();

}