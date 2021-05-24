using UnityEngine;
using XNode;

namespace Dialogue {

    [CreateAssetMenu(menuName = "Dialogue/Graph", order = 0)]
    public class DialogueGraph : NodeGraph {
        [HideInInspector]
        public DialogueBaseNode current;

        public void Restart() {
            nodes.ForEach(x => {
                if (x is Answer answer) {
                    answer.IsVisited = false;
                }
            });
            //Find the first DialogueNode without any inputs. This is the starting node.
            current = nodes.Find(x => x is StartStop startStop && startStop.function == StartStop.StartStopEnum.Start) as DialogueBaseNode;
        }

        public bool AnswerQuestion(int i) {
            if (current is Chat chat) {
                chat.AnswerQuestion(i);
                return true;
            }
            return false;
        }

    }

}