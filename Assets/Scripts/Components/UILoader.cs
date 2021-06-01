using UnityEngine;
using UnityEngine.SceneManagement;

public class UILoader : MonoBehaviour {

    private const string UI_SCENENAME = "UI";

    private void Awake() {
        if (!SceneManager.GetSceneByName(UI_SCENENAME).isLoaded) {
            SceneManager.LoadSceneAsync(UI_SCENENAME, LoadSceneMode.Additive);
        }
    }

}