using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTiltTrigger : MonoBehaviour
{
    private BalancePlatform platform;

    private void Awake()
    {
        platform = GetComponentInParent<BalancePlatform>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            platform.StartTilt();
            BalanceMinigame.Instance.playerMovement.StartBalanceAnimation();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            platform.StopTilt();
            BalanceMinigame.Instance.playerMovement.StopBalanceAnimation();
        }
    }
}
