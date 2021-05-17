using Dialogue;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueScreen : AbstractScreen<DialogueScreen> {

    public readonly UnityEvent OnSkipButtonClicked = new UnityEvent();
    public readonly UnityEvent<int> OnAnswerButtonClicked = new UnityEvent<int>();

    [Header("References")]
    [SerializeField] private GameObject speechGameObject;
    [SerializeField] private TextMeshProUGUI speechTextField;
    [SerializeField] private AnswerItem[] answerItems;
    [SerializeField] private Button skipSpeechButton;

    public void ShowSpeech(IChat chat) {
        Show();

        speechGameObject.SetActive(true);
        speechTextField.text = chat.Text;

        skipSpeechButton.gameObject.SetActive(chat.AnswerCount <= 0);
        if (chat.AnswerCount > 0) {
            ShowAnswers(chat);
        }
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

}