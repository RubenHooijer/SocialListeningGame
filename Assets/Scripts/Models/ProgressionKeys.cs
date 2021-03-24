using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName ="SLG/ProgressionKeys")]
public class ProgressionKeys : ScriptableObject {

    [Serializable]
    public class Key {
        [HideInInspector] public string Guid = System.Guid.NewGuid().ToString();
        public string Label;
    }

    public List<Key> Keys;

    public static ProgressionKeys Instance {
        get {
            if (instance == null) {
                instance = Resources.Load<ProgressionKeys>("ProgressionKeys");
            }

            return instance;
        }
    }

    private static ProgressionKeys instance;

    public Key GetKey(string guid) {
        Key key = Keys.FirstOrDefault(x => x.Guid == guid);
        return key;
    }

    public bool HasKey(string guid) {
        return Keys.Any(x => x.Guid == guid);
    }

    private void OnValidate() {
        for (int i = Keys.Count - 1; i >= 0; i--) {
            Key key = Keys[i];

            if (key.Guid == string.Empty || Keys.Count(x => x.Guid == key.Guid) > 1) {
                Key newKey = new Key();
                newKey.Label = key.Label;
                Keys[i] = newKey;
            }
        }
    }

}