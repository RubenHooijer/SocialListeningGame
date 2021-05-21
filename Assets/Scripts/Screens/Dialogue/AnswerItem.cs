using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class AnswerItem : MonoBehaviour {

    public readonly UnityEvent<int> OnButtonClickedEvent = new UnityEvent<int>();

    [Header("References")]
    [SerializeField] private TextMeshProUGUI answerTextField;
    [SerializeField] private AnimatedButton button;

    private (string Text, int Index) textWithIndex;

    public void Setup((string Text, int Index) textWithIndex) {
        this.textWithIndex = textWithIndex;
        answerTextField.SetText(textWithIndex.Text);
    }

    private void Awake() {
        button.OnClick += OnButtonClicked;
    }

    private void LateUpdate() {
        transform.position = Camera.main.WorldToScreenPoint(DialogueTransformComponent.Transform.position);
    }

    private void OnButtonClicked() {
        OnButtonClickedEvent.Invoke(textWithIndex.Index);
    }

}