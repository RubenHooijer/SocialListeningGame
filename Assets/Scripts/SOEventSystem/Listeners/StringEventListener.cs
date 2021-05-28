using UnityEngine;
using UnityEngine.Events;

public class StringEventListener : MonoBehaviour {

    [SerializeField] private StringEventChannelSO eventChannel;
    [SerializeField] private bool RemoveListenerOnDestroy;
    [SerializeField] private UnityEvent<string> onEventRaised;

    private void OnEnable() {
        if (eventChannel == null) { return; }
        eventChannel.OnEventRaised += OnEventRaised;
    }

    private void OnDisable() {
        if (RemoveListenerOnDestroy) { return; }
        if (eventChannel == null) { return; }
        eventChannel.OnEventRaised -= OnEventRaised;
    }

    private void OnDestroy() {
        if (!RemoveListenerOnDestroy) { return; }
        if (eventChannel == null) { return; }
        eventChannel.OnEventRaised -= OnEventRaised;
    }

    private void OnEventRaised(string arg0) {
        if (onEventRaised == null) { return; }
        onEventRaised.Invoke(arg0);
    }

}