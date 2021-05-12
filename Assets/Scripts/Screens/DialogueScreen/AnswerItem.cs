using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AnswerItem : ContainerItem<(string, int)> {

    public readonly UnityEvent<int> ButtonClickedEvent = new UnityEvent<int>();

    [Header("References")]
    [SerializeField] private TextMeshProUGUI answerTextField;
    [SerializeField] private Button button;

    protected override void OnSetup((string, int) textIndexPair) {
        answerTextField.SetText(textIndexPair.Item1);
    
        button.onClick.RemoveListener(OnButtonClicked);
        button.onClick.AddListener(OnButtonClicked);
    }

    protected override void OnDispose() {
        button.onClick.RemoveListener(OnButtonClicked);
    }

    private void OnButtonClicked() {
        ButtonClickedEvent.Invoke(Data.Item2);
    }

}