using XNodeEditor;

namespace DialogueEditor {

    [CustomNodeEditor(typeof(Dialogue.Exit))]
    public class ExitEditor : NodeEditor {

        public override void OnBodyGUI() {
            serializedObject.Update();

            NodeEditorGUILayout.PortField(target.GetInputPort("input"));

            serializedObject.ApplyModifiedProperties();
        }

    }

}