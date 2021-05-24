using UnityEngine.Localization;
using XNode;

namespace Dialogue {

    [NodeTint("#7a168a")][CreateNodeMenu("Answer", order =0)][NodeWidth(450)]
    public class Answer : DialogueBaseNode, IAnswer {

        public string Text => choice.GetLocalizedString();
        public bool IsHidden => (HideIfVisited && IsVisited) || (HideIfSpecificNodeIsVisited && IsSpecificNodeVisited);
        public bool IsVisited { get => isVisited; set => isVisited = value; }
        public bool HideIfVisited => hideIfVisited;
        public bool HideIfSpecificNodeIsVisited => hideIfSpecificNodeIsVisited;
        public bool IsSpecificNodeVisited => specificNode.IsVisited;

        public LocalizedString choice;
        public bool hideIfVisited = true;
        public bool hideIfSpecificNodeIsVisited = false;
        public Answer specificNode;

        private bool isVisited;

        public override void Trigger() {
            isVisited = true;
            NodePort port = GetOutputPort("output");
            for (int i = 0; i < port.ConnectionCount; i++) {
                NodePort connection = port.GetConnection(i);
                (connection.node as DialogueBaseNode).Trigger();
            }
        }

    }

}