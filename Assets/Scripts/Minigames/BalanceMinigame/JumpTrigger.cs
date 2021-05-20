using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTrigger : MonoBehaviour
{
    private BalanceMinigame balanceMinigame;
    private PlayerMovementVS player;
    private Eustachius eustachius;

    private void Awake()
    {
        balanceMinigame = BalanceMinigame.Instance;
        player = PlayerMovementVS.Instance;
        eustachius = Eustachius.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Eustachius")
        {
            balanceMinigame.EnableJumpUI();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Eustachius")
        {
            eustachius.canJump = true;
            balanceMinigame.DisableJumpUI();
        }
    }
}
