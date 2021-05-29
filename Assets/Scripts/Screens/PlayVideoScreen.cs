using UnityEngine;
using UnityEngine.Video;

public class PlayVideoScreen : AbstractScreen<PlayVideoScreen> {

    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private VideoClip deathRealmClip;
    [SerializeField] private VideoClip drowningClip;
    [SerializeField] private VoidEventChannelSO playDeathRealmVideoEvent;
    [SerializeField] private VoidEventChannelSO playDrowningVideoEvent;
    [SerializeField] private VoidEventChannelSO onVideoClipEnd;

    public void PlayDeathRealmVideo() {
        Show();
        videoPlayer.clip = deathRealmClip;
        videoPlayer.Play();
    }

    public void PlayDrowningVideo() {
        Show();
        videoPlayer.clip = drowningClip;
        videoPlayer.Play();
    }

    protected override void OnShow() {
        videoPlayer.loopPointReached += OnVideoClipFinished;
        gameObject.SetActive(true);
    }

    protected override void OnHide() {
        videoPlayer.loopPointReached -= OnVideoClipFinished;
        gameObject.SetActive(false);
    }

    private void OnVideoClipFinished(VideoPlayer source) {
        onVideoClipEnd.Raise();
    }

}