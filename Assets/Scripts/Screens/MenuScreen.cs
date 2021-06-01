using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScreen : AbstractScreen<MenuScreen> {

    private const string MUTE_SOUND_STRING = "MuteAudio";
    private const string MUTE_MUSIC_STRING = "MuteMusic";

    [SerializeField] private string musicBusKey;
    [SerializeField] private string[] soundBusKeys;

    [SerializeField, SceneName] private string sceneToStart;
    [SerializeField] private AnimatedButton startButton;
    [SerializeField] private AnimatedToggle soundToggle;
    [SerializeField] private AnimatedToggle musicToggle;

    private Bus masterBus;
    private List<Bus> soundBusses = new List<Bus>();

    protected override void Awake() {
        base.Awake();
        masterBus = RuntimeManager.GetBus(musicBusKey);
        for (int i = 0; i < soundBusKeys.Length; i++) {
            soundBusses.Add(RuntimeManager.GetBus(soundBusKeys[i]));
        }
    }

    protected override void OnShow() {
        soundToggle.ToggleValue = PlayerPrefs.GetInt(MUTE_SOUND_STRING, soundToggle.ToggleValue ? 1 : 0) == 0;
        musicToggle.ToggleValue = PlayerPrefs.GetInt(MUTE_MUSIC_STRING, musicToggle.ToggleValue ? 1 : 0) == 0;

        soundToggle.OnValueChange += OnSoundToggled;
        musicToggle.OnValueChange += OnMusicToggled;
        startButton.OnClick += OnStartButtonClicked;

        gameObject.SetActive(true);
    }

    protected override void OnHide() {
        soundToggle.OnValueChange -= OnSoundToggled;
        musicToggle.OnValueChange -= OnMusicToggled;
        startButton.OnClick -= OnStartButtonClicked;

        gameObject.SetActive(false);
    }

    private void OnStartButtonClicked() {
        SceneManager.sceneLoaded += OnSceneLoaded;
        FadeScreen.Instance.FadeToColor(Color.black, 1, () => {
            SceneManager.LoadSceneAsync(sceneToStart, LoadSceneMode.Additive);
            Hide();
        });
    }

    private void OnSoundToggled(bool isEnabled) {
        PlayerPrefs.SetInt(MUTE_SOUND_STRING, isEnabled ? 1 : 0);
        soundBusses.ForEach(x => x.setVolume(1));
    }

    private void OnMusicToggled(bool isEnabled) {
        PlayerPrefs.SetInt(MUTE_MUSIC_STRING, isEnabled ? 1 : 0);
        masterBus.setVolume(0);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
        SceneManager.SetActiveScene(scene);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}