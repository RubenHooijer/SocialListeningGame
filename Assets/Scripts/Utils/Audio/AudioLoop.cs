using System.Collections.Generic;
using UnityEngine;

public class AudioLoop {
    public string Id { get; set; }
    public bool Stopped { get; private set; }
    public AudioSource Source { get { return source; } }

    private AudioSource source;
    private float rootMaxVolume;

    private float startVolume = 1;
    private float endVolume = 1;
    private float fadeInTime;
    private float fadeOutTime = -1;
    private float time;
    private bool isMuted;

    private Dictionary<string, float> volumeModifiers = new Dictionary<string, float>();
    private List<float> cachedVolumeModifiersValues = new List<float>();

    public AudioLoop(AudioSource source) {
        time = 0;
        this.source = source;
        rootMaxVolume = source.volume;
    }

    public void Destroy() {
        Object.Destroy(source.gameObject);
    }

    public void SetMute(bool mute) {
        if (isMuted == mute) { return; }
        isMuted = mute;
        SetVolumeModifier("mute", mute ? 0.0f : 1.0f);
    }

    public void Fade(float startVolume, float endVolume, float fadeTime) {
        time = 0;
        fadeOutTime = -1;
        fadeInTime = fadeTime;
        this.startVolume = startVolume;
        this.endVolume = endVolume;
    }

    public void FadeOut(float fadeOutTime) {
        time = 0;
        fadeInTime = -1;
        this.fadeOutTime = fadeOutTime;
    }

    public void SetVolumeModifier(string id, float volume) {
        if (!volumeModifiers.ContainsKey(id)) {
            volumeModifiers.Add(id, volume);
        } else {
            cachedVolumeModifiersValues.Remove(volumeModifiers[id]);
            volumeModifiers[id] = volume;
        }
        cachedVolumeModifiersValues.Add(volume);
    }

    public void CancelFadeOut() {
        fadeOutTime = -1;
    }

    public void Update() {
        if(source == null) { return; }

        float volume = rootMaxVolume;
        time += Time.unscaledDeltaTime;

        int i = cachedVolumeModifiersValues.Count;
        while (--i > -1) {
            volume *= cachedVolumeModifiersValues[i];
        }

        if (fadeOutTime > 0) {
            float progress = Mathf.Clamp01(time / fadeOutTime);
            volume = Mathf.Lerp(volume, 0, progress);
            if (volume <= 0) {
                Stop();
            }
        } else if (fadeInTime > 0) {
            float progress = Mathf.Clamp01(time / fadeInTime);
            volume *= Mathf.Lerp(startVolume, endVolume, progress);
        }

        source.volume = volume;
    }

    public void SetPitch(float pitch) {
        if(source == null) { return; }
        source.pitch = pitch;
    }

	public void SetTime(float time) {
		source.time = time;
	}

	public float GetTime() {
		return source.time;
	}

    public void Stop() {
        if(source == null) { return; }
        source.Stop();
        Stopped = true;
    }
}