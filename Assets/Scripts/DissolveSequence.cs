using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveSequence : MonoBehaviour
{
    public ParticleSystem FirefliesParticle;
    public Animator spotLight;
    public GameObject pants;
    public GameObject sweater;
    public GameObject godRays;

    private void Start()
    {
        FirefliesParticle.Pause(true);
        spotLight.enabled = false;
        pants.SetActive(false);
        sweater.SetActive(false);
        godRays.SetActive(false);
    }
    public void startFireflies()
    {
        FirefliesParticle.Play(true);
    }

    public void focusLight()
    {
        spotLight.enabled = true;
    }

    public void fallingClothes()
    {
        pants.SetActive(true);
        sweater.SetActive(true);
        godRays.SetActive(true);
        FirefliesParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);

    }
}
