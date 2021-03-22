using Dialogue;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace DialogueEditor {

	[CustomNodeGraphEditor(typeof(DialogueGraph))]
	public class DialogueGraphEditor : NodeGraphEditor {

		public override void OnGUI() {
			if (!Application.isPlaying) { return; }

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUI.color = Color.yellow;
			if (GUILayout.Button("Highlight current node")) {
				NodeEditorWindow.current.FocusNode((Node)((DialogueGraph)NodeEditorWindow.current.graph).current);
				NodeEditorWindow.current.SelectNode((Node)((DialogueGraph)NodeEditorWindow.current.graph).current, false);
			}
			GUILayout.EndHorizontal();
		}

		public override string GetNodeMenuName(System.Type type) {
			if (type.Namespace == "Dialogue") return base.GetNodeMenuName(type).Replace("Dialogue/","");
			else return null;
		}

		public override Color GetPortColor(NodePort port) {
			Node.NodeTintAttribute tintAttrib;
			if (port.IsInput && NodeEditorUtilities.GetAttrib(port.node.GetType(), out tintAttrib)) // Return node color
				return tintAttrib.color;
			else // Return preference color
				return base.GetPortColor(port);
		}

		public override float GetNoodleThickness(NodePort output, NodePort input) {
			return (window.hoveredPort == output || window.hoveredPort == input) ? 10f : 5f;
		}

	}

}