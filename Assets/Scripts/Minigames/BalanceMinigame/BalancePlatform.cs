using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalancePlatform : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Animator eustachiusAnimator;
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private Rigidbody eustachiusRigidbody;


    private PlayerMovementVS playerMovement;
    private Eustachius eustachius;

    private InputManager inputManager;

    private BalanceMinigame balanceMinigame;

    private bool canTilt;

    private void Start()
    {
        inputManager = InputManager.Instance;
        playerMovement = PlayerMovementVS.Instance;
        eustachius = Eustachius.Instance;

        playerAnimator = playerMovement.GetComponent<Animator>();
        eustachiusAnimator = eustachius.GetComponent<Animator>();
        playerRigidbody = playerMovement.GetComponent<Rigidbody>();
        eustachiusRigidbody = eustachius.GetComponent<Rigidbody>();

        balanceMinigame = BalanceMinigame.Instance;
        canTilt = false;
    }

    public void Fall(int player)
    {
        balanceMinigame.PlayerBalanceMoveSpeed = 0;
        if(player == 0)
        {
            playerAnimator.SetBool("FallDown", true);
        }
        else
        {
            eustachiusAnimator.SetBool("FallDown", true);
        }

    }

    public void StartTilt()
    {
        playerMovement.canWalk = false;
        eustachius.canWalk = false;
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
            eustachiusRigidbody.AddForce(force);

            if (force.x < 0)
            {
                playerRigidbody.transform.rotation = Quaternion.Euler(0, 270, 0);
                eustachiusRigidbody.transform.rotation = Quaternion.Euler(0, 270, 0);

            }
            else if (force.x > 0)
            {
                playerRigidbody.transform.rotation = Quaternion.Euler(0, 90, 0);
                eustachiusRigidbody.transform.rotation = Quaternion.Euler(0, 90, 0);
            }
        }
    }
}
