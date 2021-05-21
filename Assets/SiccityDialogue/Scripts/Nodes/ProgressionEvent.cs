using XNode;

namespace Dialogue {

    [NodeTint("#737C58")][CreateNodeMenu("Progression Event", order = 6)]
	public class ProgressionEvent : DialogueBaseNode {

		public enum Action {
			Set,
			Remove
		}

		public ProgressionKey Key;
		[NodeEnum] public Action action = Action.Set;

		public override void Trigger() {
			switch (action) {
				case Action.Set:
					World.Instance.Progression.SetKey(Key);
					break;
				case Action.Remove:
					World.Instance.Progression.RemoveKey(Key);
					break;
				default:
					break;
			}

			Next();
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