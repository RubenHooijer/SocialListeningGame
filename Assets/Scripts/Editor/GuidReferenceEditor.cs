using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(STRGuid))]
public class STRGuidDrawer : PropertyDrawer {

    private class Entry {
        public readonly List<UnityEngine.Object> ResourceObjects = new List<UnityEngine.Object>();
        public readonly List<UnityEngine.Object> SceneObjects = new List<UnityEngine.Object>();
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        GUILayout.BeginHorizontal();

        property.stringValue = EditorGUILayout.TextField(property.stringValue);

        if (GUILayout.Button("New", GUILayout.Width(100))) {
            property.stringValue = Guid.NewGuid().ToString();
        }

        if (GUILayout.Button("Check", GUILayout.Width(100))) {
            Type interfaceType = typeof(IGuidable);
            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => x.IsClass && interfaceType.IsAssignableFrom(x));

            Dictionary<string, Entry> recordedObjectsPerGuid = new Dictionary<string, Entry>();

            Action<UnityEngine.Object, bool> recordGuidable = (obj, isResource) => {
                if (!(obj is IGuidable)) { return; }
                IGuidable guidable = obj as IGuidable;
                if (!recordedObjectsPerGuid.TryGetValue(guidable.Guid, out Entry entry)) {
                    entry = new Entry();
                    recordedObjectsPerGuid.Add(guidable.Guid, entry);
                }
                List<UnityEngine.Object> objects = isResource ? entry.ResourceObjects : entry.SceneObjects;
                if (objects.Contains(obj)) { return; }
                objects.Add(obj);
            };

            foreach (Type type in types) {
                // Go through all guidable objects in resources folder
                foreach (UnityEngine.Object obj in Resources.LoadAll("", type)) {
                    recordGuidable(obj, true);
                }

                // Go through all guidable objects in the current scenes
                foreach (UnityEngine.Object obj in UnityEngine.Object.FindObjectsOfType(type)) {
                    recordGuidable(obj, false);
                }
            }

            // Log duplicates
            bool encounteredDuplicates = false;
            foreach (KeyValuePair<string, Entry> pair in recordedObjectsPerGuid) {
                if (pair.Value.ResourceObjects.Count + pair.Value.SceneObjects.Count > 1) {
                    encounteredDuplicates = true;
                    Debug.Log($"<color=red>Duplicated guid '{pair.Key}' shared among:</color>");
                    pair.Value.ResourceObjects.ForEach(x => Debug.Log($"<color=orange>Resource Object: {x.name}</color>", x));
                    pair.Value.SceneObjects.ForEach(x => Debug.Log($"<color=orange>Scene Object: {x.name}</color>", x));
                }
            }

            if (!encounteredDuplicates) {
                Debug.Log("<color=green>No duplicate guids were encountered in the resources folder and current scene.</color>");
            }
        }

        GUILayout.EndHorizontal();

        property.serializedObject.ApplyModifiedProperties();
    }

}