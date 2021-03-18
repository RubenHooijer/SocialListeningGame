using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using XNode;

namespace Dialogue {
    [NodeTint("#5C7C6A")]
    public class Chat : DialogueBaseNode, IChat, ILoadableTableReference {

        public CharacterInfo character;
        public LocalizedString text;
        [Output(dynamicPortList = true)]public List<LocalizedString> answers = new List<LocalizedString>();

        TableReference ILoadableTableReference.TableReference => text.TableReference;

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