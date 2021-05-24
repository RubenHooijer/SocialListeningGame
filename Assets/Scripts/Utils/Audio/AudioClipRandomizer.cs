using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipRandomizer : MonoBehaviour {
    public AudioClip[] Clips;
    [SerializeField] private Vector2 pitchRange = Vector2.one;

    public void PlayClip() {
        if (Clips == null || Clips.Length < 1) { return; }
        AudioSource source = GetComponent<AudioSource>();
        if (source == null) {
            Debug.LogWarning("Could not find audiosource");
            return;
        }
        AudioClip randomClip = Clips[Random.Range(0, Clips.Length)];
        source.clip = randomClip;
        source.pitch = Random.Range(pitchRange.x, pitchRange.y);
        source.Play();
    }
}
