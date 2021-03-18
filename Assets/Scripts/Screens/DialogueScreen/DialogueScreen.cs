using Dialogue;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class DialogueScreen : AbstractScreen<DialogueScreen> {

    [Header("References")]
    [SerializeField] private TextMeshProUGUI speechTextField;
    [SerializeField] private GameObject textAnswersParent;
    [SerializeField] private TextAnswerItem textAnswerTemplate;
    [SerializeField] private GameObject pictureAnswersParent;
    [SerializeField] private PictureAnswerItem pictureAnswerTemplate;
    [SerializeField] private Button skipSpeechButton;

    private ItemContainer<TextAnswerItem, (LocalizedString, int)> textAnswerContainer;
    private ItemContainer<PictureAnswerItem, (LocalizedTexture2D, int)> pictureAnswerContainer;
    private DialogueGraph dialogueGraph;

    protected override void Awake() {
        base.Awake();
        textAnswerContainer = new ItemContainer<TextAnswerItem, (LocalizedString, int)>(textAnswerTemplate);
        pictureAnswerContainer = new ItemContainer<PictureAnswerItem, (LocalizedTexture2D, int)>(pictureAnswerTemplate);
    }

    public void Setup(DialogueGraph dialogueGraph) {
        dialogueGraph.Restart();
        this.dialogueGraph = dialogueGraph;
    }

    protected override void OnShow() {
        gameObject.SetActive(true);
        ShowChat(dialogueGraph.current);
    }

    protected override void OnHide() {
        gameObject.SetActive(false);
    }

    [EasyAttributes.Button]
    private void Answer0() {
        OnAnswerClicked(0);
    }

    private void OnAnswerClicked(int answerIndex) {
        Debug.Log($"You answered with {answerIndex}");
        dialogueGraph.AnswerQuestion(answerIndex);
        ShowChat(dialogueGraph.current);
    }

    private void ShowChat(IChat chat) {
        ResetOptions();
        
        if (chat == null) {
            Debug.Log($"<color=orange>You have reached an end of the dialogue</color>");
            Hide();
        } else if (chat.AnswerCount <= 0) {
            ShowSpeechOnly((Chat)chat);
        } else if (chat is Chat speechChat) {
            ShowTextChat(speechChat);
        } else if (chat is PictureChat pictureChat) {
            ShowPictureChat(pictureChat);
        } else {
            Debug.LogWarning($"Chat node couldn't be identified");
            Hide();
        }
    }

    private void ShowSpeechOnly(Chat chat) {
        speechTextField.text = chat.text.GetLocalizedString().Result;

        skipSpeechButton.gameObject.SetActive(true);
        skipSpeechButton.onClick.RemoveAllListeners();
        skipSpeechButton.onClick.AddListener(() => OnAnswerClicked(0));
    }

    private void ShowTextChat(Chat chat) {
        List<(LocalizedString, int)> textIndexPairs = new List<(LocalizedString, int)>();
        for (int i = 0; i < chat.answers.Count; i++) {
            textIndexPairs.Add((chat.answers[i], i));
        }

        speechTextField.text = chat.text.GetLocalizedString().Result;

        textAnswersParent.SetActive(true);
        textAnswerContainer.UpdateContainer(textIndexPairs);

        textAnswerContainer.Items.ForEach(x => {
            x.ButtonClickedEvent.RemoveListener(OnAnswerClicked);
            x.ButtonClickedEvent.AddListener(OnAnswerClicked);
            }
        );
    }

    private void ShowPictureChat(PictureChat pictureChat) {
        List<(LocalizedTexture2D, int)> textureIndexPairs = new List<(LocalizedTexture2D, int)>();
        for (int i = 0; i < pictureChat.answers.Length; i++) {
            textureIndexPairs.Add((pictureChat.answers[i], i));
        }

        speechTextField.text = pictureChat.text.GetLocalizedString().Result;

        pictureAnswersParent.SetActive(true);
        pictureAnswerContainer.UpdateContainer(textureIndexPairs);

        pictureAnswerContainer.Items.ForEach(x => {
            x.ButtonClickedEvent.RemoveListener(OnAnswerClicked);
            x.ButtonClickedEvent.AddListener(OnAnswerClicked);
        }
        );
    }

    private void ResetOptions() {
        textAnswersParent.SetActive(false);
        pictureAnswersParent.SetActive(false);
        skipSpeechButton.gameObject.SetActive(false);
    }

}