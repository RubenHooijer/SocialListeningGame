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

    [SerializeField] private float clickValue = 0.05f;
    [SerializeField] private float doorClickValue = 0.1f;

    private bool canClick;


    private void Start()
    {
        slider = GetComponent<Slider>();

        canClick = true;

        baseColor = handleImage.color;

        InputManager.Instance.InteractPerformed.AddListener(AddToValue);
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

        if(slider.value >= 0.5f)
        {
            speed = baseSpeed * 1.75f;
        }
        if (slider.value >= 0.75f)
        {
            speed = baseSpeed * 2.5f;
        }

        return speed;
    }

    private void AddToValue()
    {
        slider.value += 0.05f;
    }
}
