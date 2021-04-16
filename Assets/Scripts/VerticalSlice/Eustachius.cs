using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eustachius : MonoBehaviour
{
    private Animator animator;
    
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            animator.SetBool("StandUp", true);
        }
    }
}
