using Oasez.Extensions.Generics.Singleton;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine;

public class InputManager : GenericSingleton<InputManager, InputManager>
{
    public readonly UnityEvent InteractPerformed = new UnityEvent();

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
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void OnInteractPerformed(InputAction.CallbackContext obj) {
        InteractPerformed.Invoke();
    }

}