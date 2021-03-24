using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    private PlayerInput playerInput;

    private void Awake()
    {
        Instance = this;
        playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    public Vector2 GetMovement()
    {
        return playerInput.PlayerMain.Move.ReadValue<Vector2>();
    }

    public Vector2 GetLook()
    {
        return playerInput.PlayerMain.Look.ReadValue<Vector2>();
    }

    public bool GetInteract()
    {
        Debug.Log(playerInput.PlayerMain.Interact.triggered);
        return playerInput.PlayerMain.Interact.triggered;
    }
}
