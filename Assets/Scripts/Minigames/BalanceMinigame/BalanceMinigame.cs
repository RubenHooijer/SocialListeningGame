using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceMinigame : AbstractScreen<BalanceMinigame>
{
    private InputManager inputManager;

    private FadeScript fadeScript;

    [SerializeField] private float fadeTime = 3;

    private bool canJump;

    public float BalanceSpeed;

    [SerializeField] private GameObject jumpUI;

    private void Awake()
    {
        InitializeMinigame();
    }

    public void InitializeMinigame()
    {
        fadeScript = FadeScript.Instance;

        Debug.Log(fadeScript);

        inputManager = InputManager.Instance;
        inputManager.EnableInput();

        fadeScript.Fade(0, fadeTime);
    }

    public void EnableJumpUI()
    {
        canJump = true;
        jumpUI.SetActive(true);
    }

    public void DisableJumpUI()
    {
        canJump = false;
        jumpUI.SetActive(false);
    }
}
