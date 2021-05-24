using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCameraTrigger : MonoBehaviour
{
    [SerializeField] private Animator cMAnimator;

    private void OnTriggerEnter(Collider other)
    {
        cMAnimator.SetBool("DoorCam", true);
    }
}
