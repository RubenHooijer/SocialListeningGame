using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    public readonly UnityEvent OnInteractPerformed = new UnityEvent();

    public static InputManager Instance;

    private PlayerInput playerInput;

    private void Awake()
    {
        Instance = this;
        playerInput = new PlayerInput();

        //InputManager.Instance.OnInteractPerformed.AddListener(OpenDoor);
    }

    private void OnEnable()
    {
        playerInput.Enable();
        playerInput.PlayerMain.Interact.performed += Interact_performed;
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnInteractPerformed.Invoke();
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
        return playerInput.PlayerMain.Interact.triggered;
    }

}
