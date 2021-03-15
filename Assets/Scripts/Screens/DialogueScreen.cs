using Dialogue;
using EasyAttributes;
using UnityEngine;

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

    private void ShowChat(Chat chat) {
        Debug.Log($"<b>{chat.character.name}</b>:{chat.text}");
        for (int i = 0; i < chat.answers.Count; i++) {
            Debug.Log($"<b>(A{i})</b>:<color=cyan>{chat.answers[i]}</color>");
        }
    }

}