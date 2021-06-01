using NaughtyAttributes;
using XNode;

namespace Dialogue {
    [NodeTint("#786649")]
    [CreateNodeMenu("FollowPath", order = 0)]
    [NodeWidth(400)]
    public class FollowPath : DialogueBaseNode {

        public CharacterType character;
        public string pathGuid;
        public bool useCharacterSpeed = true;
        [HideIf("useCharacterSpeed")] [AllowNesting] public float speed = 1;

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