using UnityEngine;

namespace Dialogue {

    [NodeTint("#FC0352")][CreateNodeMenu("Exit", order =0)]
    public class Exit : DialogueBaseNode {

        public override void Trigger() {
            (graph as DialogueGraph).current = null;
        }

        [ContextMenu("Connect all open outputs to me")]
        private void ConnectAllOpenOutputs() {
            DialogueGraph dialogueGraph = (DialogueGraph)graph;

            for (int i = 0; i < dialogueGraph.nodes.Count; i++) {
                XNode.Node node = dialogueGraph.nodes[i];

                foreach (XNode.NodePort output in node.Outputs) {
                    if (!output.IsConnected) {
                        output.Connect(GetInputPort("input"));
                    }
                }
            }
        }

    }

}