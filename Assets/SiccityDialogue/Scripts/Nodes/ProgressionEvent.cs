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
		}

	}

}