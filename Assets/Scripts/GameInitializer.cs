using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInitializer : MonoBehaviour {

    [SerializeField, SceneName] private string[] startingScenes;

    private void Awake() {
        SceneManager.LoadScene(startingScenes[0], LoadSceneMode.Single);
        
        for (int i = 1; i < startingScenes.Length; i++) {
            SceneManager.LoadScene(startingScenes[i], LoadSceneMode.Additive);
        }
    }

}