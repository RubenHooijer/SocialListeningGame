using XNode;

namespace Dialogue {

    [NodeTint("#5C6384")][CreateNodeMenu("Progression Branch", order = 7)][NodeWidth(300)]
    public class ProgressionBranch : DialogueBaseNode {

        public ProgressionBranch.Condition[] conditions;
        [Output] public DialogueBaseNode pass;
        [Output] public DialogueBaseNode fail;

        public override void Trigger() {

            // Perform condition
            bool success = true;
            for (int i = 0; i < conditions.Length; i++) {
                Condition condition = conditions[i];

                bool hasKey = World.Instance.Progression.HasKey(condition.Key);
                success = (hasKey, condition.Check) switch
                {
                    (true, Condition.State.False) => false,
                    (false, Condition.State.True) => false,
                    _ => true
                };

                if (!success) { break; }
            }

            //Trigger next nodes
            NodePort port = success ? port = GetOutputPort("pass") : port = GetOutputPort("fail");
            if (port == null) return;
            for (int i = 0; i < port.ConnectionCount; i++) {
                NodePort connection = port.GetConnection(i);
                (connection.node as DialogueBaseNode).Trigger();
            }
        }

        [System.Serializable]
        public class Condition  {

            public enum State {
                True,
                False
            }

            public ProgressionKey Key;
            [NodeEnum] public State Check;

        }

    }

}