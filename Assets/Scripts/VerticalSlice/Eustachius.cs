using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eustachius : PlayerMovementVS
{
    public static Eustachius Instance;
    [SerializeField] public PlayerMovementVS playerMovement;

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }

    protected override void Start()
    {
        base.Start();

        canWalk = true;
        //if (DoorMinigame.Instance != null)
        //{
        //    canWalk = false;
        //}
    }

    public void StandUp()
    {
        animator.SetBool("StandUp", true);
        playerMovement.animator.SetBool("Walking", false);
        playerMovement.canWalk = false;
    }

    public void EnableWalking()
    {
        canWalk = true;
        playerMovement.canWalk = true;
    }

    protected override IEnumerator StartJump(Vector2 landPosition)
    {
        StartCoroutine(base.StartJump(landPosition));
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(WalkRight());
    }
}
