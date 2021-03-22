using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachinePOVExtension : CinemachineExtension
{
    #region Components & Variables
    //Singleton
    private InputManager inputManager;

    //Variables
    [SerializeField] private float horizontalSpeed, verticalSpeed;
    [SerializeField] private Vector2 clampValues;

    private Vector3 startingRotation;

    #endregion

    protected override void Awake()
    {
        inputManager = InputManager.Instance;
        base.Awake();
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if(vcam.Follow)
        {
            //Make Unity not generate red errors when not playing game.
            if(!Application.isPlaying)
            {
                return;
            }
            if(stage == CinemachineCore.Stage.Aim)
            {
                //Set starting rotation
                if(startingRotation == null)
                {
                    startingRotation = transform.localRotation.eulerAngles;
                }
                //Get input
                Vector2 deltaInput = inputManager.GetLook();

                //Change camera angle using input
                startingRotation.x += deltaInput.x * horizontalSpeed * Time.deltaTime;
                startingRotation.y -= deltaInput.y * verticalSpeed * Time.deltaTime;
                startingRotation.y = Mathf.Clamp(startingRotation.y, clampValues.x, clampValues.y);
                state.RawOrientation = Quaternion.Euler(startingRotation.y, startingRotation.x, 0);
            }
        }
    }
}
