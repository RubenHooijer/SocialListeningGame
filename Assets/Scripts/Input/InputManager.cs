using Oasez.Extensions.Generics.Singleton;
using UnityEngine.InputSystem;

public class InputManager : GenericSingleton<InputManager, InputManager> {

    private PlayerInput playerInput;

    protected override void Awake() {
        base.Awake();
        playerInput = new PlayerInput();

        //InputSystem.EnableDevice(Gyroscope.current);
        //InputSystem.EnableDevice(Accelerometer.current);
        //#if !UNITY_EDITOR
        //#endif
    }

    private void OnEnable() {
        playerInput.Enable();
    }

    private void OnDisable() {
        playerInput.Disable();
    }

    public float GetGyro() {
        float value = playerInput.PlayerMain.Gyro.ReadValue<float>();
        return value;
    }

}