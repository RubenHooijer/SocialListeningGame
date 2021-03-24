﻿using UnityEngine.Events;

namespace Dialogue {

	[NodeTint("#737C58")][CreateNodeMenu("Event", order =3)]
	public class Event : DialogueBaseNode {

		//public UnityEvent trigger; // If this ever fails go back to implementation below

		//public override void Trigger() {
		//	trigger.Invoke();
		//}

		public SerializableEvent[] trigger;

		public override void Trigger() {
			for (int i = 0; i < trigger.Length; i++) {
				trigger[i].Invoke();
			}
		}

	}

}