using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFallTrigger : MonoBehaviour
{
    private BalancePlatform platform;

    private void Awake()
    {
        platform = GetComponentInParent<BalancePlatform>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("asdasd");
            platform.Fall();
        }
    }
}
