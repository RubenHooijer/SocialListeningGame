using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalancePlatform : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;

    private InputManager inputManager;

    private BalanceMinigame balanceMinigame;

    [SerializeField] private bool canTilt; //TODO: Remove serializefield

    private void Start()
    {
        inputManager = InputManager.Instance;
        playerAnimator = PlayerMovementVS.Instance.transform.GetComponent<Animator>();
        balanceMinigame = BalanceMinigame.Instance;
    }

    public void Fall()
    {
        playerAnimator.SetTrigger("FallDown");
    }

    public void StartTilt()
    {
        canTilt = true;
    }

    public void StopTilt()
    {
        canTilt = false;
    }

    private void Update()
    {
        if(canTilt)
        {
            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z - (inputManager.GetGyro() * Time.deltaTime * balanceMinigame.BalanceSpeed));
        }
    }
}
