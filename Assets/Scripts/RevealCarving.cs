using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealCarving : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator RevealSpotlight;
    public Animator RevealEmission;

    private void Start()
    {
        RevealEmission.enabled = false;
        RevealSpotlight.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hoi");
        RevealSpotlight.enabled = true;
        RevealEmission.enabled = true;
    }
}
