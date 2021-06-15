using UnityEngine;

public abstract class AbstractScreen<T> : BaseScreen where T : Component {

    public static T Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<T>();
                if (_instance == null) {
                    GameObject obj = new GameObject(typeof(T).ToString() + " - Singleton");
                    _instance = obj.AddComponent<T>();
                    Debug.Log($"created a new screen instance of {typeof(T)}");
                }
            }
            return _instance;
        }
    }
    private static T _instance;

    protected virtual void Awake() {
        if (_instance == null || _instance == this) {
            _instance = this as T;
            gameObject.SetActive(false);
        } else {
            Destroy(gameObject);
        }
    }

}