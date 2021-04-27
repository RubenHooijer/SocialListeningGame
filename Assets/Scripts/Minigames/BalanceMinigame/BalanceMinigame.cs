using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceMinigame : AbstractScreen<BalanceMinigame>
{
    private InputManager inputManager;

    private FadeScript fadeScript;

    [SerializeField] private float fadeTime = 3;

    public float BalanceSpeed;

    private void OnEnable()
    {
        fadeScript = FadeScript.Instance;

        inputManager = InputManager.Instance;
        inputManager.EnableInput();

        fadeScript.Fade(0, fadeTime);
    }
}
