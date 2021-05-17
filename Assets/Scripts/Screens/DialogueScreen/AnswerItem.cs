using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AnswerItem : MonoBehaviour {

    public readonly UnityEvent<int> OnButtonClickedEvent = new UnityEvent<int>();

    [Header("References")]
    [SerializeField] private TextMeshProUGUI answerTextField;
    [SerializeField] private Button button;

    private (string Text, int Index) textWithIndex;

    public void Setup((string Text, int Index) textWithIndex) {
        this.textWithIndex = textWithIndex;
        answerTextField.SetText(textWithIndex.Text);
    }

    private void Awake() {
        button.onClick.AddListener(OnButtonClicked);
    }

    private void LateUpdate() {
        transform.position = Camera.main.WorldToScreenPoint(DialogueTransformComponent.Transform.position);
    }

    private void OnButtonClicked() {
        OnButtonClickedEvent.Invoke(textWithIndex.Index);
    }

}