using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedToggle : AnimatedButton {

    public event Action<bool> OnValueChange;

    public bool ToggleValue {
        get { return toggleValue; }
        set {
            toggleValue = value;

            UpdateVisuals();

            if (OnValueChange != null) {
                OnValueChange(value);
            }
        }
    }

    public bool IsToggleable = true;

    [Header("References")]
    [SerializeField] private List<GameObject> activeVisuals;
    [SerializeField] private List<GameObject> inactiveVisuals;

    private bool toggleValue;

    protected override void Awake() {
        base.Awake();
        UpdateVisuals();
        OnRelease += OnReleased;
        OnClick += OnClicked;
    }

    private void OnReleased() {
        if (!IsToggleable) { return; }
        ToggleValue = !toggleValue;
    }

    private void OnClicked() {
        // Empty listener so the animated button will animate
    }

    private void UpdateVisuals() {
        activeVisuals.ForEach(x => x.SetActive(toggleValue));
        inactiveVisuals.ForEach(x => x.SetActive(!toggleValue));
    }

}