using Dialogue;
using UnityEngine;

public class SceneInformation : MonoBehaviour {

    [HideInInspector] public string currentScene;
    public DialogueGraph sceneGraph;

    private void Start() {
        currentScene = gameObject.scene.name;
        GameController.Instance.SetNewGraph(this);
    }

}