using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorMinigame : AbstractScreen<DoorMinigame>
{
    private InputManager inputManager;

    public bool NearDoor;

    private GameObject minigameBar;

    private void Start()
    {
        inputManager = InputManager.Instance;
        Initialize();
    }
    
    private void Initialize()
    {
        minigameBar = GetComponentInChildren<Slider>().gameObject;
        minigameBar.SetActive(false);
        NearDoor = true;
        inputManager.InteractPerformed.AddListener(StartMinigame);
    }

    protected override void OnShow()
    {
        base.OnShow();
    }

    protected override void OnHide()
    {
        base.OnHide();
    }

    private void StartMinigame()
    {
        if (NearDoor)
        {
            minigameBar.SetActive(true);
            inputManager.InteractPerformed.RemoveListener(StartMinigame);
        }
    }
}
