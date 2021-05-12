using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameBar : MonoBehaviour
{
    //Components
    private Slider slider;
    [SerializeField] private Image handleImage;

    private Color baseColor;

    //Variables
    [SerializeField] private float baseSpeed = 0.5f;

    [SerializeField] private float doorOpenValue;
    [SerializeField] private float doorCurrentValue;

    [SerializeField] private float clickValue;
    [SerializeField] private float doorClickValue = 0.05f;

    private bool canClick;

    private DoorMinigame doorMinigame;

    private InputManager inputManager;

    private void Start()
    {
        slider = GetComponent<Slider>();

        canClick = true;

        baseColor = handleImage.color;

        clickValue = doorClickValue;

        doorMinigame = DoorMinigame.Instance;

        inputManager = InputManager.Instance;

        inputManager.InteractPerformed.AddListener(AddToValue);
    }

    private void Update()
    {
        if(slider.value > 0)
        {
            slider.value -= Time.deltaTime * CalculateSpeed();
        }
            
    }

    private float CalculateSpeed()
    {
        float speed = baseSpeed;
        clickValue = doorClickValue;

        if (slider.value >= 0.5f)
        {
            speed = baseSpeed * 1.75f;
            clickValue = doorClickValue * 2;
        }
        if (slider.value >= 0.75f)
        {
            speed = baseSpeed * 2.5f;
            clickValue = doorClickValue * 3;
        }

        return speed;
    }

    private void AddToValue()
    {
        slider.value += 0.05f;
        doorCurrentValue += clickValue;
        if(doorCurrentValue >= doorOpenValue)
        {
            doorMinigame.StartCoroutine(doorMinigame.OpenDoor());
        }
    }
}
