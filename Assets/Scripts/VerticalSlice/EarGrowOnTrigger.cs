using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarGrowOnTrigger : MonoBehaviour
{
    public Animator anim;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            anim.SetBool("Grow", true);
        }
    }
}
