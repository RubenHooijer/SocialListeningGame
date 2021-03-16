using Dialogue;
using EasyAttributes;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;

public class DialogueScreen : AbstractScreen<DialogueScreen> {

    [SerializeField] private DialogueGraph testDialogue;

    private DialogueGraph dialogueGraph;

    public void Setup(DialogueGraph dialogueGraph) {
        dialogueGraph.Restart();

        this.dialogueGraph = dialogueGraph;
    }

    protected override void OnShow() {
        gameObject.SetActive(true);
    }

    protected override void OnHide() {
        gameObject.SetActive(false);
    }

    [Button]
    private void LoadInDialogue() {
        testDialogue.Restart();
        
        ((Chat)testDialogue.current).answers[0].GetLocalizedString().Completed += x => Debug.Log($"DialogueTable has loaded in");
    }

    [Button]
    private void DebugDialogue() {
        testDialogue.Restart();
        ShowChat(testDialogue.current);
    }

    [Button]
    private void AnswerA0() {
        Debug.Log($"<color=orange>You answered <b>(A0)</b></color>");
        testDialogue.AnswerQuestion(0);
        ShowChat(testDialogue.current);
    }

    [Button]
    private void AnswerA1() {
        Debug.Log($"<color=orange>You answered <b>(A1)</b></color>");
        testDialogue.AnswerQuestion(1);
        ShowChat(testDialogue.current);
    }

    [Button]
    private void AnswerA2() {
        Debug.Log($"<color=orange>You answered <b>(A2)</b></color>");
        testDialogue.AnswerQuestion(2);
        ShowChat(testDialogue.current);
    }

    private void ShowChat(IChat chat) {
        if (chat is Chat speechChat) {
            ShowSpeechChat(speechChat);
        } else if (chat is PictureChat pictureChat) {
            ShowPictureChat(pictureChat);
        }
    }

    private void ShowSpeechChat(Chat chat) {
        Debug.Log($"<b>{chat.character.name}</b>:{chat.text.GetLocalizedString().Result}");

        for (int i = 0; i < chat.answers.Count; i++) {
            Debug.Log($"<b>(A{i})</b>:<color=cyan>{chat.answers[i].GetLocalizedString().Result}</color>");
        }
    }

    private void ShowPictureChat(PictureChat pictureChat) {
        Debug.Log($"<b>{pictureChat.character.name}</b>:{pictureChat.text.GetLocalizedString().Result}");

        for (int i = 0; i < pictureChat.answers.Length; i++) {
            Debug.Log($"<b>(A{i})</b>:<color=cyan>{pictureChat.answers[i].LoadAssetAsync().Result.name}</color>");
        }
    }

}