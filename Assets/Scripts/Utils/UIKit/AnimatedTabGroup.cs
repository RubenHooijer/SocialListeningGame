using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedTabGroup : MonoBehaviour {

    public event Action<AnimatedTab> OnTabSelect;

    public int TabCount { get { return tabs.Count; } }

    [Header("References")]
    [SerializeField] private List<AnimatedTab> tabs = default;
    [SerializeField] private AnimatedTab defaultTab = default;

    [Header("Settings")]
    [SerializeField] private bool resetOnEnable = true;

    public void ResetTabs() {
        SetSelectedTab(defaultTab);
    }

    private void OnEnable() {
        if (resetOnEnable) {
            ResetTabs();
        }
    }

    private void Awake() {
        tabs.ForEach(x => x.OnClick += OnTabClicked);
    }

    public void SetSelectedTab(int index) {
        AnimatedTab tab = tabs.GetAtIndex(index, false);
        if (tab == null) { 
            Debug.LogError("Tried to select tab at index " + index + " but index is out of bounds");
            return;
        }
        SetSelectedTab(tab);
    }

    public void AddTab(AnimatedTab tab) {
        tabs.Add(tab);
        tab.OnClick += OnTabClicked;
    }

    public void ClearTabs() {
        tabs.ForEach(x => x.OnClick -= OnTabClicked);
        tabs.Clear();
    }

    public void SetDefaultTab(AnimatedTab tab) {
        defaultTab = tab;
    }

    private void OnTabClicked(AnimatedTab tab) {
        SetSelectedTab(tab);
    }

    private void SetSelectedTab(AnimatedTab tabToSelect) {
        tabs.ForEach(x => x.SetSelected(x == tabToSelect));
        OnTabSelect?.Invoke(tabToSelect);
    }

}