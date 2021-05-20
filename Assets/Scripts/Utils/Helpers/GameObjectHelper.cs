using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameObjectHelper {

    public static GameObject InstantiateAsChild(this GameObject parent, GameObject original, bool setTransform = false) {
        if (original == null) {
            Debug.LogError("Can't instantiate from null", parent);
            return null;
        }
        GameObject g = UnityEngine.Object.Instantiate(original);
        g.transform.SetParent(parent.transform, setTransform);
        return g;
    }

    public static T InstantiateAsChild<T>(this GameObject parent, GameObject original, bool setTransform = false) where T : Component {
        if (original == null) {
            Debug.LogError("Can't instantiate from null", parent);
            return null;
        }
        GameObject g = parent.InstantiateAsChild(original, setTransform);
        T component = g.GetComponent<T>();
        if (component != null) { return component; }
        return g.GetComponentInChildren<T>();
    }
    
    public static void SetLayerRecursively(this GameObject parent, string layerName) {
        List<GameObject> children = GetAllChildGameObjects(parent);
        int layer = LayerMask.NameToLayer(layerName); ;
        parent.layer = layer;
        foreach (GameObject g in children) {
            g.layer = layer;
        }
    }

    public static T GetOrCreateComponent<T>(this GameObject gameObject) where T : Component {
        T component = gameObject.GetComponent<T>();
        if (component == null) {
            return gameObject.AddComponent<T>();
        }
        return component;
    }

    public static List<T> FindObjectsOfTypeAll<T>() {
        List<T> results = new List<T>();
        for (int i = 0; i < SceneManager.sceneCount; i++) {
            var s = SceneManager.GetSceneAt(i);
            if (s.isLoaded) {
                var allGameObjects = s.GetRootGameObjects();
                for (int j = 0; j < allGameObjects.Length; j++) {
                    var go = allGameObjects[j];
                    results.AddRange(go.GetComponentsInChildren<T>(true));
                }
            }
        }
        return results;
    }

    public static List<GameObject> GetAllGameObjects() {
        List<GameObject> gameObjects = new List<GameObject>();

        Scene scene = SceneManager.GetActiveScene();

        List<GameObject> rootGameObjects = scene.GetRootGameObjects().ToList();
        foreach (GameObject rootGameObject in rootGameObjects) {
            GetGameObjectsRecursivly(rootGameObject, gameObjects);
        }
        
        return gameObjects;
    }

    public static List<GameObject> GetAllChildGameObjects(GameObject root) {
        List<GameObject> gameObjects = new List<GameObject>();
        gameObjects.Add(root);

        GetGameObjectsRecursivly(root, gameObjects);

        return gameObjects;
    }

    private static void GetGameObjectsRecursivly(GameObject root, List<GameObject> list) {
        list.Add(root);
        for(int i = 0; i < root.transform.childCount; i++) {
            GetGameObjectsRecursivly(root.transform.GetChild(i).gameObject, list);
        }
    }

    public static void DestroyAllChildren(this GameObject parent) {
        while (parent.transform.childCount > 0) {
            GameObject.Destroy(parent.transform.GetChild(0));
        }
    }

    public static bool ContainsChild(this GameObject parent, GameObject child, bool checkParent = false) {
        if (checkParent && parent == child) { return true; }
        foreach (Transform t in parent.transform) {
            if (t.gameObject == child) { return true; }
            if (t.gameObject.ContainsChild(child)) { return true; }
        }
        return false;
    }

    public static Transform FindChildByName(Transform findIn, string name, bool strictMatch = false) {
        return FindChildByName(findIn, new string[] { name }, strictMatch);
    }
    public static Transform FindChildByName(Transform findIn, string[] names, bool strictMatch = false) {
        foreach (string name in names) {
            if (!strictMatch) {
                string nameLower = name.ToLower();
                foreach (Transform child in findIn) {
                    if (child.name.ToLower().IndexOf(nameLower) != -1)
                        return child;
                }
            } else {
                foreach (Transform child in findIn) {
                    if (child.name == name)
                        return child;
                }
            }

            //Als er niets gevonden is komt ie hier; hier kijken we naar alle subchildren
            foreach (Transform child in findIn) {
                Transform result = FindChildByName(child, names, strictMatch);
                if (result != null)
                    return result;
            }
        }

        return null;
    }

    public static T FindChildComponentByName<T>(this GameObject go, string name) where T : Component {
        Component[] results = go.GetComponentsInChildren(typeof(T),true);
        for (int i = 0; i < results.Length; i++) {
            if (results[i].name.Equals(name)) {
                return (T)results[i].GetComponent(typeof(T));
            }
        }
        return null;
    }

    public static List<T> FindChildComponentsByName<T>(this GameObject go, string name) where T : Component {
        Component[] results = go.GetComponentsInChildren(typeof(T),true);
        List<T> matchingComponents = new List<T>();
        for (int i = 0; i < results.Length; i++) {
            if (results[i].name.Equals(name)) {
                matchingComponents.Add((T)results[i].GetComponent(typeof(T)));
            }
        }
        return matchingComponents;
    }

    public static T GetComponentInParent<T>(this GameObject go, bool includeInactive) where T: Component {
        Transform transform = go.transform;
        while (transform != null) {
            if (includeInactive || transform.gameObject.activeInHierarchy) {
                T component = transform.GetComponent<T>();
                if (component != null) { return component; }
            }
            transform = transform.parent;
        }
        return null;
    }

}