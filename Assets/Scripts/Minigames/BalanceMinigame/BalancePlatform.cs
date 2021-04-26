using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalancePlatform : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;

    private InputManager inputManager;

    private void Start()
    {
        inputManager = InputManager.Instance;
    }

    public void Fall()
    {
        playerAnimator.SetTrigger("FallDown");
    }

    public void StartTilt()
    {

    }
}
