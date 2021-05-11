using Dialogue;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace DialogueEditor {

    [CustomNodeEditor(typeof(Dialogue.Answer))]
    public class AnswerEditor : NodeEditor {

        public override void OnBodyGUI() {
            serializedObject.Update();

            Answer node = target as Answer;

            EditorGUILayout.PropertyField(serializedObject.FindProperty("choice"));
            GUILayout.BeginHorizontal();
            NodeEditorGUILayout.PortField(GUIContent.none, target.GetInputPort("input"), GUILayout.MinWidth(0));
            NodeEditorGUILayout.PortField(GUIContent.none, target.GetOutputPort("output"), GUILayout.MinWidth(0));
            GUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("hideIfVisited"));

            node.hideIfSpecificNodeIsVisited = EditorGUILayout.Toggle("HideIfSpecificVisited", node.hideIfSpecificNodeIsVisited);
            if (node.hideIfSpecificNodeIsVisited) {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("specificNode"));
            }

            serializedObject.ApplyModifiedProperties();
        }

    }

}