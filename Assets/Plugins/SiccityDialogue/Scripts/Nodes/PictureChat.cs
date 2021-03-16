using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using XNode;

namespace Dialogue {
    [NodeTint("#4d8565")]
    public class PictureChat : DialogueBaseNode, IChat {

        public CharacterInfo character;
        public LocalizedString text;
        [Output(dynamicPortList = true)] public LocalizedTexture[] answers;

        public void AnswerQuestion(int index) {
            NodePort port = null;
            if (answers.Length == 0) {
                port = GetOutputPort("output");
            } else {
                if (answers.Length <= index) return;
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

    public interface IChat {

        public void AnswerQuestion(int index);

    }
}