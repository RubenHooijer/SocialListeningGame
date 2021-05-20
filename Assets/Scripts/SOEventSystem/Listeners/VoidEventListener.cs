using UnityEngine;
using UnityEngine.Events;

public class VoidEventListener : MonoBehaviour {

    [SerializeField] private VoidEventChannelSO eventChannel;
    [SerializeField] private bool RemoveListenerOnDestroy;
    [SerializeField] private UnityEvent onEventRaised;

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

    private void OnEventRaised() {
        if (onEventRaised == null) { return; }
        onEventRaised.Invoke();
    }

}