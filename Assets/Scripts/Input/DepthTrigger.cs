using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<PlayerMovementVS>().canWalkDepth = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerMovementVS>().canWalkDepth = false;
        }
    }
}
