using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlayerController : MonoBehaviour
{
    private InputManager inputManager;

    [SerializeField] private float speed;

    private void Start()
    {
        inputManager = InputManager.Instance;
    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        //float translationX = inputManager.GetMovement().x;
        float translationX = 0;

        if (translationX < 0)
        {
            transform.rotation = Quaternion.Euler(0, 270, 0);
        }
        else if (translationX > 0)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }

        transform.position += new Vector3(translationX, 0,0) * speed * Time.deltaTime;
    }
}
