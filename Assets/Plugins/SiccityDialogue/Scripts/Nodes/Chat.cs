using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Dialogue {
    [NodeTint("#5C7C6A")]
    public class Chat : DialogueBaseNode {

        public CharacterInfo character;
        [TextArea(6, 6)] public string text;
        [Output(dynamicPortList = true)][TextArea(3, 3)] public List<string> answers = new List<string>();

        [System.Serializable] public class Answer {
            public string text;
            public AudioClip voiceClip;
        }

        public void AnswerQuestion(int index) {
            NodePort port = null;
            if (answers.Count == 0) {
                port = GetOutputPort("output");
            } else {
                if (answers.Count <= index) return;
                port = GetOutputPort("answers " + index);
            }

            if (port == null) return;
            for (int i = 0; i < port.ConnectionCount; i++) {
                NodePort connection = port.GetConnection(i);
                (connection.node as DialogueBaseNode).Trigger();
            }
        }

        public override void Trigger() {
            (graph as DialogueGraph).current = this;
        }
    }
}