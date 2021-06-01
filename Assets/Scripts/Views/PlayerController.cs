using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Components & Variables
    private InputManager inputManager;
    private CharacterController controller;

    private Transform cameraMain;
    private Vector3 playerVelocity;
    //private bool groundedPlayer;

    [SerializeField] private float playerSpeed = 2.0f;
    //private float jumpHeight = 1.0f;
    //private float gravityValue = -9.81f;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        cameraMain = Camera.main.transform;
    }

    private void Start()
    {
        inputManager = InputManager.Instance;
    }

    void Update()
    {
        //Jump Logic
        /*
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        */

        //Vector2 movementInput = inputManager.GetMovement();
        Vector2 movementInput = Vector2.zero;
        Vector3 move = (cameraMain.forward * movementInput.y + cameraMain.right * movementInput.x);
        move.y = 0;
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        //Jump Logic
        /*
        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        */

        controller.Move(playerVelocity * Time.deltaTime);
    }

    #endregion
}
