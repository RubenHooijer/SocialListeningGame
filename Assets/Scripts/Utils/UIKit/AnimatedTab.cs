using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AnimatedButton))]
public class AnimatedTab : MonoBehaviour {

    public event Action<AnimatedTab> OnClick;

    public string Id { get { return gameObject.name; } }
    public bool IsSelected { get; private set; }
    
    [Header("References")]
    [SerializeField] protected List<GameObject> activeObjects;
    [SerializeField] protected List<GameObject> inactiveObjects;

    private bool isInitialized;
    private AnimatedButton animatedButton;

    public void SetSelected(bool selected) {
        InitializeIfNeeded();

        IsSelected = selected;

        animatedButton.OnClick -= OnButtonClicked;
        if (!IsSelected) {
            animatedButton.OnClick += OnButtonClicked;
        }

        activeObjects.ForEach(x => x.SetActive(selected));
        inactiveObjects.ForEach(x => x.SetActive(!selected));
    }

    private void Awake() {
        InitializeIfNeeded();
    }

    private void OnButtonClicked() {
        OnClick?.Invoke(this);
    }

    private void InitializeIfNeeded() {
        if (isInitialized) { return; }
        isInitialized = true;

        animatedButton = GetComponent<AnimatedButton>();
        SetSelected(false);
    }

}