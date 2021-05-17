using NaughtyAttributes;
using XNode;

namespace Dialogue {

    [NodeTint("#A8A8A8")]
    [CreateNodeMenu("Wait", order = 0)]
    [NodeWidth(250)]
    public class Wait : DialogueBaseNode {

        public WaitFor waitFor = WaitFor.Time;

        [HideIf("notWaitingForTime")][AllowNesting] public float Time = 1f;
        [HideIf("notWaitingForPathEnd")][AllowNesting] public string PathGuid;

        private bool notWaitingForTime => waitFor != WaitFor.Time;
        private bool notWaitingForPathEnd => waitFor != WaitFor.PathEnd;

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

    public enum WaitFor {

        Time    = 0,
        PathEnd = 1,

    }

}