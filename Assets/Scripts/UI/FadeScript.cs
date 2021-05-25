using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class FadeScript : MonoBehaviour
{
    public static FadeScript Instance;
    private Image fadeImage;

    [SerializeField] private float time = 5;

    private void Awake()
    {
        Instance = this;
        fadeImage = GetComponent<Image>();
        Fade(0, time);
    }

    /// <summary>
    /// Use direction paramater to decide whether to fade in(1) or out(0).
    /// </summary>
    /// <param name="direction"></param>
    public void Fade(int direction, float duration)
    {
        fadeImage.DOFade(direction, duration);
    }
}
