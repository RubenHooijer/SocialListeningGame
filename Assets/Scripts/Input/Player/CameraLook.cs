using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineFreeLook))]
public class CameraLook : MonoBehaviour
{

    private CinemachineFreeLook cinemachine;

    private PlayerInput playerInput;

    [SerializeField] private float lookSpeedX, lookSpeedY = 1;

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
        Vector2 delta = playerInput.PlayerMain.Look.ReadValue<Vector2>();
        cinemachine.m_XAxis.Value = delta.x * lookSpeedX * Time.deltaTime;
        cinemachine.m_YAxis.Value = delta.y * lookSpeedY * Time.deltaTime;
    }
}
