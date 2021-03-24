using EasyAttributes;
using UnityEngine;

public class TestInteractableNPC : MonoBehaviour {

    [SerializeField] private CharacterType npc;

    [Button]
    public void Interact() {
        DialogueScreen.Instance.Setup(npc.DialogueGraph);
        DialogueScreen.Instance.Show();
    }

}