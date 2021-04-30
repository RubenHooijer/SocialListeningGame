using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceMinigame : AbstractScreen<BalanceMinigame>
{
    private InputManager inputManager;

    private FadeScript fadeScript;

    private PlayerMovementVS playerMovement;

    [SerializeField] private float fadeTime = 3;

    private bool canJump;

    public float BalanceSpeed;

    public float PlayerBalanceMoveSpeed;

    [SerializeField] private GameObject jumpUI;

    [SerializeField] private List<Transform> balancePlatforms;

    [SerializeField] private Vector2 balancePlatformLandOffset;

    private int currentPlatform;

    private void Start()
    {
        InitializeMinigame();
    }

    public void InitializeMinigame()
    {
        fadeScript = FadeScript.Instance;

        playerMovement = PlayerMovementVS.Instance;

        Debug.Log(fadeScript);

        inputManager = InputManager.Instance;
        inputManager.EnableInput();

        inputManager.JumpPerformed.AddListener(JumpToPlatform);

        fadeScript.Fade(0, fadeTime);

        currentPlatform = 0;
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

    private void JumpToPlatform()
    {
        if (canJump)
        {
            canJump = false;
            playerMovement.Jump((Vector2)balancePlatforms[currentPlatform].position + balancePlatformLandOffset);
            currentPlatform++;
        }
    }
}
