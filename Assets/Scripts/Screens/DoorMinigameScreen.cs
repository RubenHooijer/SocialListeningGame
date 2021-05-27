using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class DoorMinigameScreen : AbstractScreen<DoorMinigameScreen> {

    [System.Serializable]
    public class PushThreshold {
        public float Threshold;
        [Range(1, 10)] public float Resistance;
    }

    [Header("Settings")]
    [SerializeField] private float baseClickValue = 0.05f;
    [SerializeField] private PushThreshold[] pushThresholds;

    [Header("References")]
    [SerializeField] private Slider pushSlider;
    [SerializeField] private Animator handleAnimator;
    [SerializeField, AnimatorParam("handleAnimator", AnimatorControllerParameterType.Bool)] private string isWalkingAnimationParameter;
    [SerializeField] private AnimatedButton pushButton;
    [SerializeField] private AnimatedWidget[] animatedWidgets;
    [SerializeField] private VoidEventChannelSO pushMinigameCompleted;

    private float sliderValue;

    protected override void OnShow() {
        gameObject.SetActive(true);
        pushButton.OnClickDirect += OnButtonClicked;
    }

    protected override void OnHide() {
        pushButton.OnClickDirect -= OnButtonClicked;
        animatedWidgets.Foreach(x => x.Hide());

        CoroutineHelper.Delay(2.5f, () => gameObject.SetActive(false));
    }

    private void OnButtonClicked() {
        PushSlider();
    }

    private float GetClickValue() {
        float resistance = 1;

        for (int i = 0; i < pushThresholds.Length; i++) {
            if (sliderValue >= pushThresholds[i].Threshold) {
                resistance = pushThresholds[i].Resistance;
            }
        }

        return baseClickValue / resistance;
    }

    private void PushSlider() {
        float clickValue = GetClickValue();
        sliderValue += clickValue;

        pushSlider.DOKill();
        pushSlider.DOValue(sliderValue, 0.71f)
            .SetEase(Ease.Linear)
            .OnStart(OnStartPush)
            .OnUpdate(() => {
                if (pushSlider.value >= (pushSlider.maxValue * 0.98f)) { OnEndPush(); }
            })
            .OnComplete(OnEndPush);
    }

    private void OnStartPush() {
        handleAnimator.SetBool(isWalkingAnimationParameter, true);
    }

    private void OnEndPush() {
        handleAnimator.SetBool(isWalkingAnimationParameter, false);
        if (sliderValue >= (pushSlider.maxValue * 0.98f)) {
            EndGame();
            return; 
        }
        pushSlider.DOValue(0, 8)
            .SetDelay(0.05f)
            .SetEase(Ease.Linear)
            .OnUpdate(() => {
                sliderValue = pushSlider.value;
            });
    }

    private void EndGame() {
        pushButton.OnClickDirect -= OnButtonClicked;
        Hide();
        pushMinigameCompleted.Raise();
    }

}