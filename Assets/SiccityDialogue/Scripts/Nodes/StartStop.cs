using XNode;

namespace Dialogue {
    [NodeTint("#f5ef42")]
	[CreateNodeMenu("StartStop", order = 3)]
	public class StartStop : DialogueBaseNode {

		public enum StartStopEnum {
			Start,
			Stop
		}

		public StartStopEnum function;

		public override void Trigger() {
			((DialogueGraph)graph).current = this;
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