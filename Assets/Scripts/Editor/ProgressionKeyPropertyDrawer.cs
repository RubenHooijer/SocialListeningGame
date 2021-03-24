using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ProgressionKey))]
public class ProgressionKeyPropertyDrawer : PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.BeginProperty(position, label, property);

        List<string> optionLabels = new List<string>(ProgressionKeys.Instance.Keys.Select(x => x.Label));
        List<string> optionGuids = new List<string>(ProgressionKeys.Instance.Keys.Select(x => x.Guid));

        optionLabels.Insert(0, "[EMPTY]");
        optionGuids.Insert(0, "");

        string[] optionsArray = optionLabels.ToArray();
        string[] optionGuidsArray = optionGuids.ToArray();

        int index = Array.IndexOf(optionGuidsArray, property.FindPropertyRelative("guid").stringValue);
        int newIndex = EditorGUI.Popup(position, label.text, index, optionsArray);

        if (index != newIndex) {
            string key;
            if (newIndex == 0) {
                key = "";
            } else {
                key = optionGuids[newIndex];
            }

            property.FindPropertyRelative("guid").stringValue = key;
        }

        EditorGUI.EndProperty();
    }


}