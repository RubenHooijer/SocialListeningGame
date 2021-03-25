using UnityEngine;

public class NPC : MonoBehaviour, IInteractable {

    [SerializeField] private CharacterType npc;

    public void Interact() {
        DialogueScreen.Instance.Setup(npc.DialogueGraph);
        DialogueScreen.Instance.Show();
    }

}