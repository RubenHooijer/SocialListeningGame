﻿using UnityEngine.Localization;
using XNode;

namespace Dialogue {

    [NodeTint("#4F0349")][CreateNodeMenu("Answer", order =0)][NodeWidth(250)]
    public class Answer : DialogueBaseNode, IAnswer {

        public string Text => choice.GetLocalizedString().Result;
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

        private void Awake() {
            isVisited = false;
        }

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