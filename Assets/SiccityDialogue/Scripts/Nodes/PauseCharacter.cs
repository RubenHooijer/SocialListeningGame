using NaughtyAttributes;
using XNode;

namespace Dialogue {
    [NodeTint("#786649")]
    [CreateNodeMenu("PauseCharacter", order = 0)]
    [NodeWidth(200)]
    public class PauseCharacter : DialogueBaseNode {

        public CharacterType character;
        [Dropdown("GetBoolValues")] public bool pause = false;

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

        private DropdownList<bool> GetBoolValues() {
            return new DropdownList<bool>()
            {
                { "Unpause", true},
                { "Pause", false },
            };
        }

    }

}