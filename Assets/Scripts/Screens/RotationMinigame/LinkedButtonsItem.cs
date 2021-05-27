using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LinkedButtonsItem : ContainerItem<string> {

    [Header("Settings")]
    [SerializeField] private float colorLerpDuration = 0.3f;
    [SerializeField] private Color showingColor;
    [SerializeField] private Color hiddenColor;

    [Header("References")]
    [SerializeField] private AnimatedButton upButton;
    [SerializeField] private Image upButtonImage;
    [SerializeField] private AnimatedButton downButton;
    [SerializeField] private Image downButtonImage;

    public void SetInteractable(bool isInteractable) {
        upButton.Animate = isInteractable;
        downButton.Animate = isInteractable;

        upButton.IsInteractable = isInteractable;
        downButton.IsInteractable = isInteractable;

        upButtonImage.DOColor(isInteractable ? showingColor : hiddenColor, colorLerpDuration);
        downButtonImage.DOColor(isInteractable ? showingColor : hiddenColor, colorLerpDuration);
    }

    protected override void OnSetup(string data) {
        upButton.OnClick += () => RotationMinigameScreen.Instance.RotateCylinder(data, true);
        downButton.OnClick += () => RotationMinigameScreen.Instance.RotateCylinder(data, false);
    }

    protected override void OnDispose() {
        upButton.OnClick -= () => RotationMinigameScreen.Instance.RotateCylinder(Data, true);
        downButton.OnClick -= () => RotationMinigameScreen.Instance.RotateCylinder(Data, false);
    }

}