using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnimatedButtonMediator : Button{

    public event Action OnDown;
    public event Action OnExit;
    public event Action OnUp;
    public event Action OnClick;

    public override void OnPointerDown(PointerEventData eventData) {
        base.OnPointerDown(eventData);
        OnDown?.Invoke();
    }

    public override void OnPointerExit(PointerEventData eventData) {
        base.OnPointerExit(eventData);
        OnExit?.Invoke();
    }

    public override void OnPointerUp(PointerEventData eventData) {
        base.OnPointerUp(eventData);
        OnUp?.Invoke();
    }

    protected override void Awake() {
        base.Awake();
        onClick.AddListener(() => {
            OnClick?.Invoke();
        });
    }

}