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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            platform.StopTilt();
        }
    }
}
