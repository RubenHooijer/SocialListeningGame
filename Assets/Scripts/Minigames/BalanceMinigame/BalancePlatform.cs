using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalancePlatform : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Rigidbody playerRigidbody;

    private PlayerMovementVS playerMovement;

    private InputManager inputManager;

    private BalanceMinigame balanceMinigame;

    private bool canTilt;

    private void Start()
    {
        inputManager = InputManager.Instance;
        playerMovement = PlayerMovementVS.Instance;

        playerAnimator = playerMovement.transform.GetComponent<Animator>();
        playerRigidbody = playerMovement.transform.GetComponent<Rigidbody>();

        balanceMinigame = BalanceMinigame.Instance;
        canTilt = false;
    }

    public void Fall()
    {
        inputManager.DisableInput();
        balanceMinigame.PlayerBalanceMoveSpeed = 0;
        playerAnimator.SetBool("FallDown", true);

    }

    public void StartTilt()
    {
        playerMovement.canWalk = false;
        canTilt = true;
    }

    public void StopTilt()
    {
        canTilt = false;
    }

    private void Update()
    {
        if (canTilt)
        {
            transform.localRotation = Quaternion.Euler(transform.rotation.eulerAngles.x + (inputManager.GetGyro() * Time.deltaTime * balanceMinigame.BalanceSpeed), 180, 0);

            //calculate angle in - and + degrees;
            float angle = transform.localEulerAngles.x;
            angle = (angle > 180) ? angle - 360 : angle;

            Vector3 force = transform.forward * angle * balanceMinigame.PlayerBalanceMoveSpeed * Time.deltaTime;
            playerRigidbody.AddForce(force);
            if (force.x < 0)
            {
                playerRigidbody.transform.rotation = Quaternion.Euler(0, 270, 0);
            }
            else if (force.x > 0)
            {
                playerRigidbody.transform.rotation = Quaternion.Euler(0, 90, 0);
            }
        }
    }
}
