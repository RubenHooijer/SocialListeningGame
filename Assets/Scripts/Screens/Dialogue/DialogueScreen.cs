using DG.Tweening;
using Dialogue;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueScreen : AbstractScreen<DialogueScreen> {

    [Serializable]
    public class CharacterGroup {
        [SerializeField] public string Characters;
        [SerializeField] public float WaitDuration;
    }

    private const string TRANSPARENT_COLOR_RICHTEXT_PREFIX = "<color=#0000>";
    private const string COLOR_RICHTEXT_SUFFIX = "</color>";

    public readonly UnityEvent OnSkipButtonClicked = new UnityEvent();
    public readonly UnityEvent<int> OnAnswerButtonClicked = new UnityEvent<int>();

    [Header("Settings")]
    [SerializeField] private float defaultCharacterWaitDuration = 0.025f;
    [SerializeField] private List<CharacterGroup> characterGroups;

    [Header("References")]
    [SerializeField] private GameObject speechGameObject;
    [SerializeField] private Image speechBackground;
    [SerializeField] private TextMeshProUGUI speechTextField;
    [SerializeField] private Image characterIcon;
    [SerializeField] private TextMeshProUGUI characterNameTextField;
    [SerializeField] private AnswerItem[] answerItems;
    [SerializeField] private Button skipSpeechButton;

    public void ShowSpeech(IChat chat) {
        Show();

        skipSpeechButton.gameObject.SetActive(false);
        
        speechGameObject.SetActive(true);
        speechTextField.text = string.Empty;
        StartCoroutine(
            TypeWriterEffectRoutine(chat.Text,
                () => {
                    skipSpeechButton.gameObject.SetActive(chat.AnswerCount <= 0);
                    if (chat.AnswerCount > 0) {
                        CoroutineHelper.Delay(0.4f, () => ShowAnswers(chat));
                    }
                })
        );
        speechBackground.color = chat.Character.DialogueColor;

        characterIcon.sprite = chat.Character.Icon;
        characterNameTextField.text = chat.Character.Name;
    }

    protected override void Awake() {
        base.Awake();
        speechGameObject.SetActive(false);
        EnableAnswers(false);

        for (int i = 0; i < answerItems.Length; i++) {
            answerItems[i].OnButtonClickedEvent.AddListener(OnAnswerClicked);
        }
    }

    protected override void OnShow() {
        gameObject.SetActive(true);
        skipSpeechButton.onClick.AddListener(OnSkipButtonClicked.Invoke);
    }

    protected override void OnHide() {
        gameObject.SetActive(false);
        skipSpeechButton.onClick.RemoveListener(OnSkipButtonClicked.Invoke);
    }

    private void OnAnswerClicked(int answerIndex) {
        OnAnswerButtonClicked.Invoke(answerIndex);
        EnableAnswers(false);
    }

    private void ShowAnswers(IChat chat) {
        for (int i = 0; i < chat.Answers.Count; i++) {
            Answer answer = chat.Answers[i];
            AnswerItem answerItem = answerItems[i];

            answerItem.Setup((answer.Text, i));
            answerItem.gameObject.SetActive(true);
        }
    }

    private void EnableAnswers(bool enabled) {
        for (int i = 0; i < answerItems.Length; i++) {
            answerItems[i].gameObject.SetActive(enabled);
        }
    }

    private IEnumerator TypeWriterEffectRoutine(string text, Action onSpeechCompleted) {
        int currentIndex = 0;

        CharacterGroup previousCharacterGroup = null;

        float timeTillNextCharacter = 0.0f;
        while (currentIndex < text.Length) {
            timeTillNextCharacter -= Time.deltaTime;

            while (timeTillNextCharacter <= 0.0f) {
                speechTextField.text = text.Insert(currentIndex + 1, TRANSPARENT_COLOR_RICHTEXT_PREFIX) + COLOR_RICHTEXT_SUFFIX;
                currentIndex++;

                if (currentIndex >= text.Length - 1) { break; }

                char nextCharacter = text[currentIndex];
                CharacterGroup nextCharacterGroup = characterGroups.Find(x => x.Characters.Contains(nextCharacter.ToString()));
                if (previousCharacterGroup == nextCharacterGroup) {
                    timeTillNextCharacter += defaultCharacterWaitDuration;
                } else {
                    float waitDuration = previousCharacterGroup == null ? defaultCharacterWaitDuration : previousCharacterGroup.WaitDuration;
                    timeTillNextCharacter += waitDuration;
                    previousCharacterGroup = nextCharacterGroup;
                }
            }

            yield return null;
        }

        onSpeechCompleted?.Invoke();
    }

}