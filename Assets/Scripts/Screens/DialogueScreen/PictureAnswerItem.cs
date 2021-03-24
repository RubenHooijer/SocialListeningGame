using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PictureAnswerItem : ContainerItem<(LocalizedTexture2D, int)> {

    public readonly UnityEvent<int> ButtonClickedEvent = new UnityEvent<int>();

    [Header("References")]
    [SerializeField] private Image answerImage;
    [SerializeField] private Button button;

    protected override void OnSetup((LocalizedTexture2D, int) textureIndexPair) {
        Texture2D texture = textureIndexPair.Item1.LoadAssetAsync().Result;
        answerImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

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