﻿using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using XNode;

namespace Dialogue {

    [NodeTint("#7C5C7B")][CreateNodeMenu("Picture Chat", order =2)]
    public class PictureChat : DialogueBaseNode, IChat, ILoadableTableReference {

        public CharacterInfo character;
        public LocalizedString text;
        [Output(dynamicPortList = true)] public LocalizedTexture2D[] answers;

        int IChat.AnswerCount => answers.Length;
        TableReference ILoadableTableReference.TableReference => answers[0].TableReference;

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

}