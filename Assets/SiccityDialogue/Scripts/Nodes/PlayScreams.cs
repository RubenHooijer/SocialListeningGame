using FMODUnity;
using UnityEngine;
using XNode;

namespace Dialogue {
    [NodeTint("#34dbeb")]
    [CreateNodeMenu("Play Eu screams", order = 0)]
    [NodeWidth(450)]
    public class PlayScreams : DialogueBaseNode {

        [EventRef] public string sound;
        public float spawnRateChangeDuration = 2f;
        public float volumeDuration = 2f;
        public string spawnRateName;
        [Range(0, 1)] public float endSpawnRate;
        [Range(0, 1)] public float endVolume;

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