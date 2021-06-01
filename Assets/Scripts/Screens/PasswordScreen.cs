using DG.Tweening;
using TMPro;
using UnityEngine;

public class PasswordScreen : AbstractScreen<PasswordScreen> {

    [SerializeField] private string correctWord;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private AnimatedButton enterButton;

    protected override void OnShow() {
        enterButton.OnClickDirect += OnEnterButtonClicked;

        gameObject.SetActive(true);
    }

    protected override void OnHide() {
        enterButton.OnClickDirect -= OnEnterButtonClicked;

        gameObject.SetActive(false);
    }

    private void OnEnterButtonClicked() {
        if (inputField.text == correctWord) {
            OnCorrectPassword();
        } else {
            OnWrongPassword();
        }
    }

    private void OnCorrectPassword() {
        FadeScreen.Instance.FadeToColor(Color.black, 2, () => {
            MenuScreen.Instance.Show();
            Hide();
            FadeScreen.Instance.FadeFromBlack();
        });
    }

    private void OnWrongPassword() {
        inputField.transform.DOShakePosition(0.4f, 20, 20);
        inputField.text = string.Empty;
    }

}