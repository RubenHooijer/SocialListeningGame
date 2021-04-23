using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneTilt : MonoBehaviour
{
    [SerializeField] private float speed = 10;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,0, -speed * Time.deltaTime);

        if(transform.rotation.eulerAngles.z <= 70)
        {
            speed *= -1;
        }
        if(transform.rotation.eulerAngles.z >= 110)
        {
            speed *= -1;
        }
    }
}
