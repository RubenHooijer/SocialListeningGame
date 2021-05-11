using System.Linq;
using UnityEngine;
using XNode;

namespace Dialogue {

    [CreateAssetMenu(menuName = "Dialogue/Graph", order = 0)]
    public class DialogueGraph : NodeGraph {
        [HideInInspector]
        public IDialogueNode current;

        public void Restart() {
            //Find the first DialogueNode without any inputs. This is the starting node.
            current = nodes.Find(x => x is IChat && x.Inputs.All(y => !y.IsConnected)) as IChat;
        }

        public bool AnswerQuestion(int i) {
            if (current is Chat chat) {
                chat.AnswerQuestion(i);
                return true;
            }
            return false;
        }

        public void Next() {
            if (current is Chat chat) {
                ((Chat)chat.GetPort("output").Connection.node).Trigger();
            }
        }

    }

}