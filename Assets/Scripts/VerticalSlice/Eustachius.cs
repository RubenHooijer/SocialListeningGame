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
}
