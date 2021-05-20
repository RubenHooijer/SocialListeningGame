using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Audio : MonoBehaviour {

    [Serializable]
    private class MusicAudioSource {
        public string ID = null;
        public AudioSource Source = null;
    }

    private static readonly Dictionary<string, AudioLoop> loopingSounds = new Dictionary<string, AudioLoop>();
    private static readonly List<AudioLoop> cachedLoopingSoundsValues = new List<AudioLoop>();
    private static readonly List<string> loopsToRemove = new List<string>();
    private static readonly List<string> preloadedSubfolders = new List<string>();

    private static Audio instance;
    private static string lastPlayedMusic;

    [Header("Resources")]
    [SerializeField] private AudioSource ambienceSound;

    [Header("Music References")]
    [SerializeField][Tooltip("Music to play if no other music is playing, like ambience. Keep empty for silence.")] private string defaultMusicId;
    [SerializeField] private MusicAudioSource[] musicSources;

    public static AudioLoop PlayMusic(string id) {
        InitializeIfNeeded();
        StopLoop(instance.defaultMusicId);
        AudioLoop loop = null;
        lastPlayedMusic = id;
        foreach (MusicAudioSource musicSource in instance.musicSources) {
            if (musicSource.ID == id) {
                loop = PlayLoop(musicSource.Source, musicSource.ID);
            } else {
                StopLoop(musicSource.ID);
            }
        }
        return loop;
    }

    public static void StopMusic(bool pause = false) {
        InitializeIfNeeded();
        foreach (MusicAudioSource musicSource in instance.musicSources) {
            StopLoop(musicSource.ID);
        }

        if (!pause) {
            lastPlayedMusic = null;
        }

        PlayLoop(instance.ambienceSound, instance.defaultMusicId);
    }

    public static void ContinueMusic() {
        PlayMusic(lastPlayedMusic);
	}

	public static AudioSource PlaySound(AudioSource original) {
        InitializeIfNeeded();
        if (original == null) { return null; }
        
        AudioClipInstanceLimiter limiter = original.gameObject.GetComponent<AudioClipInstanceLimiter>();
        if (limiter != null) {
            if (limiter.MaxInstances <= limiter.CurrentInstanceAmount) {
                return null;
            }
        }
        
        AudioSource source = instance.gameObject.InstantiateAsChild<AudioSource>(original.gameObject);
        
        AudioClipInstanceLimiter sourceLimiter = source.gameObject.GetComponent<AudioClipInstanceLimiter>();
        if (sourceLimiter != null) {
            sourceLimiter.AddInstance();
        }
        
        AudioClipRandomizer randomizer = source.GetComponent<AudioClipRandomizer>();
        if (randomizer != null) {
            randomizer.PlayClip();
        }
        
        CoroutineHelper.Start(DestroyOnFinish(source));

        return source;
    }

    public static AudioLoop PlayLoop(AudioSource original, string id) {
        InitializeIfNeeded();

        if (original == null) { return null; }
        if (id != null) {
            if (loopingSounds.ContainsKey(id)) {
                loopingSounds[id].CancelFadeOut();
                return loopingSounds[id];
            }
        }

        AudioSource source = instance.gameObject.InstantiateAsChild<AudioSource>(original.gameObject);
        source.loop = true;
        AudioLoop loop = new AudioLoop(source);
        if (id != null) {
            loop.Id = id;
            loopingSounds.Add(id, loop);
            cachedLoopingSoundsValues.Add(loop);
        }
        return loop;
    }

    public static void StopLoop(string id, bool instant = false) {
        if (id == null) { return; }
        InitializeIfNeeded();
        if (!loopingSounds.ContainsKey(id)) { return; }
        AudioLoop loop = loopingSounds[id];
        if (instant) {
            loop.Stop();
        } else {
            loop.FadeOut(0.5f);
        }
    }

    private static void InitializeIfNeeded() {
        if (instance != null) { return; }
        instance = FindObjectOfType<Audio>();
    }

    private void Awake() {
        InitializeIfNeeded();
    }

    private void Update() {
        for (int i = cachedLoopingSoundsValues.Count - 1; i >= 0; i--) { 
            AudioLoop loop = cachedLoopingSoundsValues[i];
            loop.Update();
            if (loop.Stopped) {
                loopsToRemove.Add(loop.Id);
            }
        }

        for (int i = loopsToRemove.Count - 1; i >= 0; i--) {
            string id = loopsToRemove[i];
            AudioLoop loop;
            if (!loopingSounds.TryGetValue(id, out loop)) { continue; }
            loopingSounds.Remove(id);
            cachedLoopingSoundsValues.Remove(loop);
            loop.Destroy();
        }
        loopsToRemove.Clear();
    }

    private static IEnumerator DestroyOnFinish(AudioSource source) {
        while (source.isPlaying) {
            yield return new WaitForEndOfFrame();
        }
        Destroy(source.gameObject);
    }

}