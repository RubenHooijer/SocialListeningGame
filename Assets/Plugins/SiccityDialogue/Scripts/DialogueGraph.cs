using System.Linq;
using UnityEngine;
using XNode;

namespace Dialogue {
    [CreateAssetMenu(menuName = "Dialogue/Graph", order = 0)]
    public class DialogueGraph : NodeGraph {
        [HideInInspector]
        public IChat current;

        public void Restart() {
            //Find the first DialogueNode without any inputs. This is the starting node.
            current = nodes.Find(x => x is IChat && x.Inputs.All(y => !y.IsConnected)) as IChat;
        }

        public IChat AnswerQuestion(int i) {
            if (current == null) { return null; }
            IChat previousChat = current;

            current.AnswerQuestion(i);
            current = current != previousChat ? current : null;

            return current;
        }
    }
}