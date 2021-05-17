using NaughtyAttributes;
using XNode;
using UnityEngine;
using System.Linq;

namespace Dialogue {

    [NodeTint("#7875ff")]
    [CreateNodeMenu("Play Animation", order = 0)]
    [NodeWidth(250)]
    public class PlayAnimationEvent : DialogueBaseNode {

        public IntEventChannelSO trigger;
        [AnimatorParam("GetAnimatorWithEvent", AnimatorControllerParameterType.Trigger)] public int animationTrigger;

        public override void Trigger() {
            (graph as DialogueGraph).current = this;
        }

        public void Next() {
            NodePort port = GetOutputPort("output");
            for (int i = 0; i < port.ConnectionCount; i++) {
                NodePort connection = port.GetConnection(i);
                (connection.node as DialogueBaseNode).Trigger();
            }
        }

        private Animator GetAnimatorWithEvent() {
            PlayAnimationEventListener[] animationListeners = FindObjectsOfType<PlayAnimationEventListener>();
            return animationListeners.First(x => x.EventChannel == trigger).Animator;
        }

    }

}