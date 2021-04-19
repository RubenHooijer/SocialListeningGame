using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomTrigger : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private ParticleSystem particles;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            animator.SetTrigger("PlayerLand");
        }
    }

    public void StartEffect()
    {
        particles.Stop();
        particles.Play();
    }
}
