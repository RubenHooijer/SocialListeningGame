using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMinigameDoor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            DoorMinigame.Instance.NearDoor = true;
        }
    }
}
