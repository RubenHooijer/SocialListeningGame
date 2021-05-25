using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    [SerializeField] private Sprite AudioImage, AudioMuteImage, MusicImage, MusicMuteImage;

    [SerializeField] private Image AudioButton, MusicButton;

    private void Start()
    {
        CheckSoundSettings();
    }

    private void CheckSoundSettings()
    {
        //Audio
        if (PlayerPrefs.GetInt("MuteAudio") == 0)
        {
            AudioButton.sprite = AudioImage;
        }
        else
        {
            AudioButton.sprite = AudioMuteImage;
        }

        //Music
        if (PlayerPrefs.GetInt("MuteMusic") == 0)
        {
            MusicButton.sprite = MusicImage;
        }
        else
        {
            MusicButton.sprite = MusicMuteImage;
        }
    }

    public void LoadScene()
    {
        StartCoroutine(LoadSceneCoroutine());
    }

    private IEnumerator LoadSceneCoroutine()
    {
        FadeScript.Instance.Fade(1, 3);

        yield return new WaitForSeconds(3);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void MuteAudio()
    {
        if(PlayerPrefs.GetInt("MuteAudio") == 0)
        {
            AudioButton.sprite = AudioMuteImage;
            PlayerPrefs.SetInt("MuteAudio", 1);
        }
        else
        {
            AudioButton.sprite = AudioImage;
            PlayerPrefs.SetInt("MuteAudio", 0);
        }
    }

    public void MuteMusic()
    {
        if (PlayerPrefs.GetInt("MuteMusic") == 0)
        {
            MusicButton.sprite = MusicMuteImage;
            PlayerPrefs.SetInt("MuteMusic", 1);
        }
        else
        {
            MusicButton.sprite = MusicImage;
            PlayerPrefs.SetInt("MuteMusic", 0);
        }
    }
}
