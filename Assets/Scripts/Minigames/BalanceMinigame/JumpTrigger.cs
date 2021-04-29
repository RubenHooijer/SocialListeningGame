using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTrigger : MonoBehaviour
{
    private BalanceMinigame balanceMinigame;

    private void Awake()
    {
        balanceMinigame = BalanceMinigame.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            balanceMinigame.EnableJumpUI();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            balanceMinigame.EnableJumpUI();
        }
    }
}
