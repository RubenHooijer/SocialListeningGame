using NaughtyAttributes;
using UnityEngine;
using XNode;

namespace Dialogue {
    [NodeTint("#e30b00")]
    [CreateNodeMenu("CameraNode", order = 0)]
    [NodeWidth(250)]
    public class Camera : DialogueBaseNode {

        public enum Action {
            LookAt = 0,
            CameraOffset = 1,
        }

        public Action action = Action.LookAt;
        [HideIf("notWaitingForLookAt")] [AllowNesting] public string lookAtGuid;
        [HideIf("notWaitingForCameraOffset")] [AllowNesting] public Vector3 offset;
        [HideIf("notWaitingForCameraOffset")] [AllowNesting] public float duration = 1;

        private bool notWaitingForLookAt => action != Action.LookAt;
        private bool notWaitingForCameraOffset => action != Action.CameraOffset;

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

    }


}