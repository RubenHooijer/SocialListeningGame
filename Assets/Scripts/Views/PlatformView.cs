using DG.Tweening;
using Dialogue;
using FMOD.Studio;
using FMODUnity;
using NaughtyAttributes;
using Oasez.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformView : MonoBehaviour, IGuidable {

    public string Guid => guid;
    private readonly static List<PlatformView> Views = new List<PlatformView>();

    [SerializeField] [STRGuid] private string guid;

    [Header("Audio")]
    [SerializeField, EventRef] private string tiltAudio;

    [Header("Settings")]
    [SerializeField] private float imbalanceMultiplier = 0.003f;
    [SerializeField] private float tiltMultiplier = 0.0105f;
    [SerializeField, Range(-1, 1)] private float balance = 0f;
    [SerializeField, MinMaxSlider(-1, 1)] private Vector2 inBalance;
    [SerializeField] private float neededTimeInBalance = 2;
    [SerializeField] private float maxRotationAngle = 30;

    [Header("References")]
    [SerializeField] private string balanceAnimationFloat = "InBalance";
    [SerializeField] private Transform platform;
    [SerializeField] private Transform[] characterPositions;

    [Header("Events")]
    [SerializeField] private CharacterIntEventChannelSO placeCharacterOnPlatformEvent;
    [SerializeField] private StringEventChannelSO startBalancingEvent;
    [SerializeField] private VoidEventChannelSO onCompletedBalancingEvent;

    [Header("Testing")]
    [SerializeField] private bool isBalancing = true;
    [SerializeField, Disable] private float imbalanceForce;
    [SerializeField, Disable] private float tiltForce;
    [SerializeField, Disable] private float timeInBalance;

    private List<CharacterView> charactersOnPlatform = new List<CharacterView>();
    private EventInstance tiltAudioInstance;

    [Button]
    private void DebugCompleteBalancing() {
        isBalancing = false;
        OnCompletedBalancing();
    }

    public static PlatformView GetView(string guid) {
        return Views.Find(x => x.Guid == guid);
    }

    public void SetCharacterToPosition(CharacterType character , int positionIndex) {
        CharacterView characterView = CharacterView.GetView(character);
        Transform positionTransform = characterPositions[positionIndex];

        characterView.transform.SetParent(positionTransform);
        characterView.transform.SetGlobalScale(Vector3.one);
        characterView.transform.localPosition = Vector3.zero;
        characterView.transform.localRotation = Quaternion.identity;

        charactersOnPlatform.Add(characterView);
    }

    private void OnEnable() {
        Views.Add(this);
        placeCharacterOnPlatformEvent.OnEventRaised += OnPlaceCharacterOnPlatform;
        startBalancingEvent.OnEventRaised += OnStartBalancing;
    }

    private void OnDisable() {
        Views.Remove(this);
        placeCharacterOnPlatformEvent.OnEventRaised -= OnPlaceCharacterOnPlatform;
        startBalancingEvent.OnEventRaised -= OnStartBalancing;
    }

    private void OnValidate() {
        ShowBalance();
    }

    private void OnPlaceCharacterOnPlatform(CharacterType character, int positionIndex, string guid) {
        if (this.guid != guid) { return; }

        SetCharacterToPosition(character, positionIndex);
    }

    private void OnStartBalancing(string guid) {
        if (this.guid != guid) { return; }
        StartBalancing();
    }

    private void OnCompletedBalancing() {
        onCompletedBalancingEvent.Raise();
        DOTween.To(() => balance, x => balance = x, 0, 1.4f).SetEase(Ease.InOutSine)
            .OnUpdate(ShowBalance);
        tiltAudioInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    private void StartBalancing() {
        isBalancing = true;
        tiltAudioInstance = RuntimeManager.CreateInstance(tiltAudio);
        tiltAudioInstance.set3DAttributes(RuntimeManager.Listeners[0].transform.To3DAttributes());
        tiltAudioInstance.start();
        StartCoroutine(BalancingRoutine());
    }

    private void ShowBalance() {
        platform.localEulerAngles = new Vector3(-balance * maxRotationAngle, 0, 0);
    }

    private void UpdateSound(float tiltSpeed) {
        float positiveBalance = Mathf.Abs(balance);
        tiltAudioInstance.setParameterByName("plateau_tilt", positiveBalance);
        float roundedTiltSpeed = positiveBalance == 1 ? 1 : Mathf.Abs(tiltSpeed * 100);
        tiltAudioInstance.setParameterByName("plateau_tilt_speed", roundedTiltSpeed);
    }

    private void UpdateCharacterBalancing() {
        for (int i = 0; i < charactersOnPlatform.Count; i++) {
            Animator animator = charactersOnPlatform[i].Animator;
            if (animator == null) { continue; }

            animator.SetFloat(balanceAnimationFloat, Input.acceleration.x, 0.2f, Time.deltaTime);
        }
    }

    private IEnumerator BalancingRoutine() {
        bool isLeaningLeft = Random.value > 0.5f;
        imbalanceForce = isLeaningLeft ? -imbalanceMultiplier : imbalanceMultiplier * Time.deltaTime;
        tiltForce = 0;

        DOTween.To(
            () => inBalance,
            x => inBalance = x,
            new Vector2(-1.5f, 1.5f),
            23f);

        while (isBalancing) {
            tiltForce = Input.acceleration.x * tiltMultiplier;

            imbalanceForce += ((balance < 0) ? -imbalanceMultiplier : imbalanceMultiplier) * Time.deltaTime;
            imbalanceForce = Mathf.Abs(balance) == 1 ? 0 : imbalanceForce;
            imbalanceForce += tiltForce * Time.deltaTime;

            balance += imbalanceForce;
            balance = Mathf.Clamp(balance, -1, 1);
            UpdateSound(imbalanceForce);
            ShowBalance();
            UpdateCharacterBalancing();

            if (balance > inBalance.x && balance < inBalance.y) {
                timeInBalance += Time.deltaTime;
            } else {
                timeInBalance = 0;
            }

            if (timeInBalance >= neededTimeInBalance) {
                isBalancing = false;
                OnCompletedBalancing();
                yield break;
            }
            yield return null;
        }
    }

}