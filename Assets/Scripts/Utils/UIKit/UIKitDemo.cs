using System.Collections.Generic;
using UnityEngine;

public class UIKitDemo : MonoBehaviour {

    [SerializeField] private List<GameObject> scenes;

    private int activeSceneIndex;

    private void Awake() {
        UpdateScenes();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            activeSceneIndex++;
            UpdateScenes();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            activeSceneIndex--;
            UpdateScenes();
        }
    }

    private void UpdateScenes() {
        GameObject activeScene = scenes.GetAtIndex(activeSceneIndex);
        scenes.ForEach(x => x.SetActive(x == activeScene));
    }

}