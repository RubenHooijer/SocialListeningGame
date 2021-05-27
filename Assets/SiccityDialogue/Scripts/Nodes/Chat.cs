using System.Collections.Generic;
using UnityEngine.Localization;
using XNode;

namespace Dialogue {

    [NodeTint("#5C7C6A")]
    [CreateNodeMenu("Chat", order = 1)]
    [NodeWidth(400)]
    public class Chat : DialogueBaseNode, IChat {

        public string Text => text.GetLocalizedString();
        public int AnswerCount => answers.Count;
        public List<Answer> Answers {
            get {
                List<Answer> availableAnswers = new List<Answer>();
                for (int i = 0; i < answers.Count; i++) {
                    if (!answers[i].IsHidden) {
                        availableAnswers.Add(answers[i]);
                    }
                }
                return availableAnswers;
            }
        }
        public CharacterType Character => character;

        public CharacterType character;
        public LocalizedString text;
        [Output(dynamicPortList = true)] public List<Answer> answers = new List<Answer>();

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

        public override void OnCreateConnection(NodePort from, NodePort to) {
            base.OnCreateConnection(from, to);
            string portName = from.fieldName;
            if (!portName.Contains("answers")) { return; }

            if (to.node is Answer answer && int.TryParse(portName[portName.Length - 1].ToString(), out int answerIndex)) {
                answers[answerIndex] = answer;
            }
        }

    }

}