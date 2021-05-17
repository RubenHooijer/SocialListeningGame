using UnityEngine;

public class PlayAnimationEventListener : MonoBehaviour {

    [SerializeField] private IntEventChannelSO eventChannel;
    [SerializeField] private Animator animator;

    public IntEventChannelSO EventChannel => eventChannel;
    public Animator Animator => animator;

    private void OnEnable() {
        if (eventChannel == null) { return; }
        eventChannel.OnEventRaised += OnEventRaised;
    }

    private void OnDisable() {
        if (eventChannel == null) { return; }
        eventChannel.OnEventRaised -= OnEventRaised;
    }

    private void OnEventRaised(int animationTrigger) {
        animator.SetTrigger(animationTrigger);
    }

}