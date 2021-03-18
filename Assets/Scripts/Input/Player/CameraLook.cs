using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineFreeLook))]
public class CameraLook : MonoBehaviour
{

    private CinemachineFreeLook cinemachine;

    private PlayerInput playerInput;

    [SerializeField] private float lookSpeedX = 1, lookSpeedY = 1;
    [SerializeField] private Vector2 YClampValues = new Vector2(-0.25f, 0.75f);

    private void Awake()
    {
        playerInput = new PlayerInput();
        cinemachine = GetComponent<CinemachineFreeLook>();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    void Update()
    {
        //Get Input
        Vector2 delta = playerInput.PlayerMain.Look.ReadValue<Vector2>();

        //X Axis
        cinemachine.m_XAxis.Value = delta.x * lookSpeedX * Time.deltaTime;

        //Y Axis
        float UnclampedY = cinemachine.GetRig(1).GetCinemachineComponent<CinemachineComposer>().m_ScreenY + delta.y * lookSpeedY * Time.deltaTime;
        float ClampedY = Mathf.Clamp(UnclampedY, YClampValues.x, YClampValues.y);
        cinemachine.GetRig(1).GetCinemachineComponent<CinemachineComposer>().m_ScreenY = ClampedY;
        
    }
}
