using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EustachiusStandupTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Eustachius.Instance.StandUp();
        }
    }
}
