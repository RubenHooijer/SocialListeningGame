using NaughtyAttributes;

namespace Dialogue {

    [NodeTint("#bb9dcc")]
    [CreateNodeMenu("Load Scene", order = 0)]
    [NodeWidth(250)]
    public class LoadScene : DialogueBaseNode {

        [Scene] public string nextScene;

        public override void Trigger() {
            (graph as DialogueGraph).current = this;
        }

    }

}