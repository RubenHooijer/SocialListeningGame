using UnityEngine;
using UnityEngine.Events;

public class VoidEventListener : MonoBehaviour {

    [SerializeField] private VoidEventChannelSO eventChannel;
    [SerializeField] private UnityEvent onEventRaised;

    private void OnEnable() {
        if (eventChannel == null) { return; }
        eventChannel.OnEventRaised += OnEventRaised;
    }

    private void OnDisable() {
        if (eventChannel == null) { return; }
        eventChannel.OnEventRaised -= OnEventRaised;
    }

    private void OnEventRaised() {
        if (onEventRaised == null) { return; }
        onEventRaised.Invoke();
    }

}