using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;
using FMODUnity;

public class AnimatedButton : MonoBehaviour, IAnimatingWidget {

    public event Action OnDown;
    public event Action OnRelease;
    public event Action OnClickDirect;
    public event Action OnClick;
    public event Action<AnimatedButton> OnClickReferenced;

    public Transform Visual { get { return visualOverride; } set { visualOverride = value; } }
    public bool Animate { get { return animate; } set { animate = value; } }

    public bool IsInteractable { get {
            return mediator == null ? isInteractable : mediator.interactable;
        }
        set {
            isInteractable = value;
            if(mediator != null) {
                mediator.interactable = isInteractable;
            }
        }
    }

    private const float PRESS_DURATION = .1f;
    private const float RELEASE_DURATION = .2f;
    private const float CLICK_IN_DURATION = .02f;
    private const float CLICK_OUT_DURATION = .1f;
    private const float PRESS_SCALE = .85f;
    private const float CLICK_SCALE = .8f;
    private const float PUSH_BACK_DISTANCE = .4f;

    private AnimatedButtonMediator mediator;
    private bool isInteractable = true;

    [Header("Resources")]
    [SerializeField, EventRef] private string soundpathClick;

    [Header("References")]
    [SerializeField] private Transform visualOverride = default;

    [Header("Settings")]
    [SerializeField][Tooltip("Whether this button should animate in and out at all")] private bool animate = true;
    [SerializeField][Tooltip("Animate even though nothing is listening to the click.")] private bool animateWithoutListeners = default;
    public bool IgnoreOtherAnimatedWidgets = false;
    public bool IgnoreClickDelay = false;

    public void Click() {
        if (OnClick == null &&
            OnClickReferenced == null &&
            OnClickDirect == null &&
            OnRelease == null &&
            !animateWithoutListeners) {
            return;
        }
        
        if (string.IsNullOrEmpty(soundpathClick) == false) {
            RuntimeManager.PlayOneShot(soundpathClick);
        }

        if (!animate) {
            OnClick?.Invoke();
            OnClickReferenced?.Invoke(this);
            return;
        }

        if (IgnoreClickDelay) {
            OnClick?.Invoke();
            OnClickReferenced?.Invoke(this);

            if (animate) {
                GetTransform().DOKill();
                GetTransform().DOScale(1.0f, RELEASE_DURATION);
            }

            return;
        }

        AnimatedWidget.AddAnimating(this);

        GetTransform().DOKill();
        GetTransform().DOScale(CLICK_SCALE, CLICK_IN_DURATION)
            .SetEase(Ease.OutExpo)
            .OnComplete(() => {
                GetTransform().DOScale(1.0f, CLICK_OUT_DURATION)
                    .SetEase(Ease.InOutSine)
                    .OnComplete(() => {
                        AnimatedWidget.RemoveAnimating(this);
                        OnClick?.Invoke();
                        OnClickReferenced?.Invoke(this);
                    });
            });
    }

    protected virtual void Awake() {
        CreateMediator();
    }

    protected virtual void OnDestroy() {
        Transform transform = GetTransform();
        if (transform != null) {
            transform.DOKill();
        }
    }

    private void CreateMediator() {
        mediator = gameObject.GetOrCreateComponent<AnimatedButtonMediator>();
        mediator.transition = Selectable.Transition.None;
        mediator.OnClick += OnUIButtonClick;
        mediator.OnDown += OnUIButtonDown;
        mediator.OnExit += OnUIButtonExit;
        mediator.interactable = isInteractable;
    }

    private void OnUIButtonDown() {
        if (AnimatedWidget.IsAnimating && !IgnoreOtherAnimatedWidgets) { return; }

        OnClickDirect?.Invoke();

        if (OnClick != null || OnClickReferenced != null || OnClickDirect != null || animateWithoutListeners) {
            OnDown?.Invoke();

            if (animate) {
                GetTransform().DOKill();
                GetTransform().DOScale(PRESS_SCALE, PRESS_DURATION);
            }
        }
    }

    private void OnUIButtonClick() {
        if (AnimatedWidget.IsAnimating && !IgnoreOtherAnimatedWidgets) { return; }
        OnRelease?.Invoke();
        Click();
    }

    private void OnUIButtonExit() {
        if (AnimatedWidget.IsAnimating && !IgnoreOtherAnimatedWidgets) { return; }
        if (animate) {
            GetTransform().DOKill();
            GetTransform().DOScale(1.0f, RELEASE_DURATION);
        }
    }

    private Transform GetTransform() {
        if (visualOverride != null) { return visualOverride; }
        return transform;
    }

}