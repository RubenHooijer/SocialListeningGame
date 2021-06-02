using DG.Tweening;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class RotationMinigameScreen : AbstractScreen<RotationMinigameScreen> {

    private const float DEFAULT_ROTATE_AMOUNT = 60;
    private const float HALF_ROTATION_AMOUNT = 180;

    [Header("Audio")]
    [SerializeField, EventRef] private string rotationSound;

    [Header("Settings")]
    [SerializeField] private float rotationDuration = 0.5f;

    [Header("References")]
    [SerializeField] private AnimatedButton lockButton;
    [SerializeField] private LinkedButtonsItem linkedButtonsTemplate;
    [SerializeField] private string[] cylinderGuids;
    [SerializeField] private float[] correctRotationSequence;
    [SerializeField] private AnimatedWidget[] animatedWidgets;

    [Header("Events")]
    [SerializeField] private VoidEventChannelSO onLockButtonClicked;

    private ItemContainer<LinkedButtonsItem, string> linkedButtonsContainer;
    private EventInstance rotationAudioInstance;

    public void RotateCylinder(string guid, bool isUp) {
        CylinderView staticView = CylinderView.GetView(guid);
        Vector3 rotation = new Vector3(isUp ? DEFAULT_ROTATE_AMOUNT : -DEFAULT_ROTATE_AMOUNT, 0, 0);

        staticView.transform.DOBlendableRotateBy(rotation, rotationDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.OutBack, 1f);
        rotationAudioInstance.set3DAttributes(RuntimeManager.Listeners[0].transform.To3DAttributes());
        DOTween.To(
            () => { rotationAudioInstance.getParameterByName("speed", out float value); return value; },
            x => rotationAudioInstance.setParameterByName("speed", x),
            0.7f,
            0.4f)
            .OnComplete(() =>
                DOTween.To(
                () => { rotationAudioInstance.getParameterByName("speed", out float value); return value; },
                x => rotationAudioInstance.setParameterByName("speed", x),
                0f,
                0.3f)
            );
    }

    public void ShowButtons(string guid) {
        lockButton.IsInteractable = true;
        lockButton.Animate = true;
        SetLinkedButtonsInteractable(guid, true);
    }

    public void HideButtons(string guid) {
        SetLinkedButtonsInteractable(guid, false);
    }

    protected override void Awake() {
        base.Awake();
        linkedButtonsContainer = new ItemContainer<LinkedButtonsItem, string>(linkedButtonsTemplate);
        linkedButtonsContainer.UpdateContainer(cylinderGuids);

        rotationAudioInstance = RuntimeManager.CreateInstance(rotationSound);
    }

    protected override void OnShow() {
        lockButton.OnClick += OnLockButtonClicked;

        gameObject.SetActive(true);
        linkedButtonsContainer.Items.ForEach(x => x.SetInteractable(false));
        rotationAudioInstance.start();
    }

    protected override void OnHide() {
        lockButton.OnClick -= OnLockButtonClicked;

        animatedWidgets.Foreach(x => x.Hide());
        CoroutineHelper.Delay(.2f, () => gameObject.SetActive(false));
        rotationAudioInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    private void OnLockButtonClicked() {
        lockButton.IsInteractable = false;
        lockButton.Animate = false;
        onLockButtonClicked.Raise();
        CheckCombination();
    }

    private void CheckCombination() {
        for (int i = 0; i < cylinderGuids.Length; i++) {
            CylinderView staticView = CylinderView.GetView(cylinderGuids[i]);
            staticView.transform.rotation.ToAngleAxis(out float xAngle, out Vector3 vector3);

            float xQuaternionRotation = vector3.x;
            xQuaternionRotation = xQuaternionRotation == Mathf.Infinity ? 1 : xQuaternionRotation;
            xQuaternionRotation = xQuaternionRotation == Mathf.NegativeInfinity ? -1 : xQuaternionRotation;

            xAngle = Mathf.RoundToInt(xAngle * xQuaternionRotation);

            if (!IsCorrectAngle(xAngle, correctRotationSequence[i])) {
                return;
            }
        }
        LightUpCombination();
    }

    private void LightUpCombination() {
        for (int i = 0; i < cylinderGuids.Length; i++) {
            CylinderView staticView = CylinderView.GetView(cylinderGuids[i]);
            staticView.Renderer.material.DOColor(Color.white * 4, "_EmissionColor", 1.2f).SetEase(Ease.InOutSine);
        }
    }

    private void SetLinkedButtonsInteractable(string guid, bool isInteractable) {
        LinkedButtonsItem linkedButtons = linkedButtonsContainer.Get(guid);
        if (linkedButtons == null) {
            Debug.Log("No linked buttons found with id " + guid);
            return;
        }

        linkedButtons.SetInteractable(isInteractable);
    }

    private bool IsCorrectAngle(float angle, float correctRotation) {
        float startingCorrectRotation = correctRotation;
        angle = angle == 360 ? 0 : angle;

        for (int i = 0; i < 2; i++) {
            correctRotation += HALF_ROTATION_AMOUNT * i;

            if (correctRotation == angle) {
                return true;
            }
        }
        correctRotation = startingCorrectRotation;
        for (int i = 0; i < 2; i++) {
            correctRotation -= HALF_ROTATION_AMOUNT;

            if (correctRotation == angle) {
                return true;
            }
        }

        return false;
    }

}