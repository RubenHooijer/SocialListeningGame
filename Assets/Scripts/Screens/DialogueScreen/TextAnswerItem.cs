using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.UI;

public class TextAnswerItem : ContainerItem<(LocalizedString, int)> {

    public readonly UnityEvent<int> ButtonClickedEvent = new UnityEvent<int>();

    [Header("References")]
    [SerializeField] private TextMeshProUGUI answerTextField;
    [SerializeField] private Button button;

    protected override void OnSetup((LocalizedString, int) textIndexPair) {
        answerTextField.SetText(textIndexPair.Item1.GetLocalizedString().Result);
    
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