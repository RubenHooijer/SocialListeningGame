using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eustachius : PlayerMovementVS
{
    public static Eustachius Instance;

    private void Awake()
    {
        base.Awake();
        Instance = this;
    }

    private void StandUp()
    {
        animator.SetBool("StandUp", true);
    }

    protected override IEnumerator StartJump(Vector2 landPosition)
    {
        StartCoroutine(base.StartJump(landPosition));
        yield return new WaitForSeconds(2.5f);
        Debug.Log("walk");
        StartCoroutine(WalkRight());
    }
}
