using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceMinigame : AbstractScreen<BalanceMinigame>
{
    private InputManager inputManager;

    private FadeScript fadeScript;

    public PlayerMovementVS playerMovement;
    private Eustachius eustachius;


    [SerializeField] private float fadeTime = 3;

    private bool canJump;

    public float BalanceSpeed;

    public float PlayerBalanceMoveSpeed;

    [SerializeField] private GameObject jumpUI;

    [SerializeField] public List<Transform> balancePlatforms;

    [SerializeField] public Vector2 balancePlatformLandOffset;

    public int currentPlatform;

    private bool movingToNextPlatform;

    private void Start()
    {
        InitializeMinigame();
    }

    public void InitializeMinigame()
    {
        fadeScript = FadeScript.Instance;

        playerMovement = PlayerMovementVS.Instance;
        eustachius = Eustachius.Instance;

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
        if (eustachius.canJump)
        {
            eustachius.canJump = false;
            if(currentPlatform >= balancePlatforms.Count)
            {
                return;
            }
            eustachius.Jump((Vector2)balancePlatforms[currentPlatform].position + balancePlatformLandOffset);
            playerMovement.StartCoroutine(playerMovement.WalkRight());
            //eustachius.StartCoroutine(eustachius.WalkRight());
        }
    }

    public IEnumerator NextPlatform()
    {
        if(!movingToNextPlatform)
        {
            Debug.Log("in");
            movingToNextPlatform = true;
            currentPlatform++;
        }
        yield return new WaitForSeconds(1f);
        movingToNextPlatform = false;
    }
}
