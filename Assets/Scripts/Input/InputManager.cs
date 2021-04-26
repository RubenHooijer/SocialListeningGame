using Oasez.Extensions.Generics.Singleton;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine;

public class InputManager : GenericSingleton<InputManager, InputManager>
{
    public readonly UnityEvent InteractPerformed = new UnityEvent();
    public readonly UnityEvent JumpPerformed = new UnityEvent();

    private PlayerInput playerInput;

    public Vector2 GetMovement()
    {
        return playerInput.PlayerMain.Move.ReadValue<Vector2>();
    }

    public Vector2 GetLook()
    {
        return playerInput.PlayerMain.Look.ReadValue<Vector2>();
    }

    protected override void Awake() {
        base.Awake();
        playerInput = new PlayerInput();

        playerInput.PlayerMain.Interact.performed += OnInteractPerformed;
        playerInput.PlayerMain.Jump.performed += OnJumpPerformed;

    }

    private void OnEnable()
    {
        EnableInput();
    }

    private void OnDisable()
    {
        DisableInput();
    }

    public void EnableInput()
    {
        playerInput.Enable();
    }

    public void DisableInput()
    {
        playerInput.Disable();
    }

    private void OnInteractPerformed(InputAction.CallbackContext obj) {
        InteractPerformed.Invoke();
    }

    private void OnJumpPerformed(InputAction.CallbackContext obj)
    {
        JumpPerformed.Invoke();
    }

    private Vector2 GetGyro()
    {
        return Vector2.zero;
    }

}