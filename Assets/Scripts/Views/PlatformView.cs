using DG.Tweening;
using Dialogue;
using NaughtyAttributes;
using Oasez.Extensions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlatformView : MonoBehaviour, IGuidable {

    private readonly static List<PlatformView> Views = new List<PlatformView>();
    public string Guid => guid;

    [SerializeField] [STRGuid] private string guid;

    [Header("Settings")]
    [SerializeField] private float imbalanceMultiplier = 0.003f;
    [SerializeField] private float tiltMultiplier = 0.0105f;
    [SerializeField, Range(-1, 1)] private float balance = 0f;
    [SerializeField, MinMaxSlider(-1, 1)] private Vector2 inBalance;
    [SerializeField] private float neededTimeInBalance = 2;
    [SerializeField] private float maxRotationAngle = 30;

    [Header("References")]
    [SerializeField] private Transform platform;
    [SerializeField] private Transform[] characterPositions;

    [Header("Testing")]
    [SerializeField] private CharacterType testCharacter;
    [SerializeField] private CharacterType testCharacter2;
    [SerializeField] private bool isBalancing = true;
    [SerializeField, Range(-.7f, .7f)] private float gyroValue;
    [SerializeField, Disable] private float imbalanceForce;
    [SerializeField, Disable] private float tiltForce;
    [SerializeField, Disable] private float timeInBalance;
    [SerializeField] private TextMeshProUGUI DebugText;

    [Button]
    void TestJumpTo0Position() {
        JumpToPosition(testCharacter, 0);
    }

    [Button]
    void TestJumpTo2Position() {
        JumpToPosition(testCharacter2, 2);
    }

    private void Start() {
        CoroutineHelper.Delay(3, () => StartBalancing());
    }

    [Button]
    void StartBalancing() {
        StartCoroutine(BalancingRoutine());
        Input.gyro.enabled = true;
    }

    public static PlatformView GetView(string guid) {
        return Views.Find(x => x.Guid == guid);
    }

    public void JumpToPosition(CharacterType character , int positionIndex) {
        CharacterView characterView = CharacterView.GetView(character);
        Transform positionTransform = characterPositions[positionIndex];

        //characterView.transform.position = positionTransform.position;
        characterView.transform.SetParent(positionTransform, false);
        characterView.transform.localPosition = Vector3.zero;
        characterView.transform.localRotation = Quaternion.identity;
    }

    private void Update() {
        DebugText.text =
            $"IsAvailable: {SystemInfo.supportsGyroscope}\n" +
            $"RotRate: {Input.gyro.rotationRate}\n" +
            $"Attitude: {Input.gyro.attitude}\n" +
            $"Euler: {Input.gyro.attitude.eulerAngles}\n" +
            $"Accel: {Input.acceleration}";
    }

    private void OnEnable() {
        Views.Add(this);
        Application.targetFrameRate = 60;
    }

    private void OnDisable() {
        Views.Remove(this);
    }

    private void OnValidate() {
        ShowBalance();
    }

    private void OnCompletedBalancing() {
        DOTween.To(() => balance, x => balance = x, 0, 1.4f).SetEase(Ease.InOutSine)
            .OnUpdate(ShowBalance);
    }

    private void ShowBalance() {
        platform.localEulerAngles = new Vector3(-balance * maxRotationAngle, 0, 0);
    }

    private IEnumerator BalancingRoutine() {
        bool isLeaningLeft = Random.value > 0.5f;
        imbalanceForce = isLeaningLeft ? -imbalanceMultiplier : imbalanceMultiplier * Time.deltaTime;
        tiltForce = 0;

        while (isBalancing) {
            //tiltForce = Input.acceleration.x * tiltMultiplier;
            tiltForce = gyroValue * tiltMultiplier;
            imbalanceForce += ((balance < 0) ? -imbalanceMultiplier : imbalanceMultiplier) * Time.deltaTime;
            imbalanceForce += tiltForce * Time.deltaTime;
            balance += imbalanceForce;
            balance = Mathf.Clamp(balance, -1, 1);
            ShowBalance();

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