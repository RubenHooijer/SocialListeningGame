using XNode;

namespace Dialogue {
    [NodeTint("#7875ff")]
    [CreateNodeMenu("Character Int String Event", order = 0)]
    [NodeWidth(350)]
    public class CharacterIntStringEvent : DialogueBaseNode {

        public CharacterIntEventChannelSO trigger;
        public CharacterType character;
        public int number;
        public string guid;

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