using BansheeGz.BGSpline.Curve;
using Dialogue;
using NaughtyAttributes;
using UnityEngine;

public class GameController : MonoBehaviour {

    [SerializeField] private DialogueGraph startingDialogue;
    [SerializeField] private BGCurve startingPath;
    [SerializeField] private WalkerComponent player;

    [Button]
    public void StartScene() {
        player.SetPath(startingPath);
        player.EnableMovement(true);
        DialogueScreen.Instance.Setup(startingDialogue);
        DialogueScreen.Instance.Show();
    }


}