using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToDoorTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Eustachius")
        {
            StartCoroutine(Eustachius.Instance.MoveToDoor());
            StartCoroutine(Eustachius.Instance.playerMovement.MoveToDoor());
        }
    }
}
