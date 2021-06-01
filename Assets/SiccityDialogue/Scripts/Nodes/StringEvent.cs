using XNode;

namespace Dialogue {
    [NodeTint("#7875ff")]
    [CreateNodeMenu("String Event", order = 0)]
    [NodeWidth(250)]
    public class StringEvent : DialogueBaseNode {

        public StringEventChannelSO trigger;
        public string data;

        public override void Trigger() {
            (graph as DialogueGraph).current = this;
        }

        public void Next() {
            NodePort port = GetOutputPort("output");
            for (int i = 0; i < port.ConnectionCount; i++) {
                NodePort connection = port.GetConnection(i);
                (connection.node as DialogueBaseNode).Trigger();
            }
        }

    }

}