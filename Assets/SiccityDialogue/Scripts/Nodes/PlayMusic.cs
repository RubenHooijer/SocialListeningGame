using FMODUnity;
using XNode;

namespace Dialogue {
    [NodeTint("#34dbeb")]
    [CreateNodeMenu("Play Music", order = 0)]
    [NodeWidth(350)]
    public class PlayMusic : DialogueBaseNode {

        [EventRef] public string music;

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