using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace DialogueEditor {
    [CustomNodeEditor(typeof(Dialogue.Exit))]
    public class ExitEditor : NodeEditor {

        public override void OnBodyGUI() {
            serializedObject.Update();

            NodeEditorGUILayout.PortField(target.GetInputPort("input"), GUILayout.Width(40));

            serializedObject.ApplyModifiedProperties();
        }

        public override int GetWidth() {
            return 80;
        }
    }
}