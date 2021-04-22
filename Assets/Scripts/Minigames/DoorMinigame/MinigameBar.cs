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
    [SerializeField] private float baseSpeed = 1f;

    [SerializeField] private float doorOpenValue;
    [SerializeField] private float doorCurrentValue;

    [SerializeField] private float ClickCooldown = 1;

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
        slider.value = Mathf.PingPong(Time.time * baseSpeed, 1);

        //Reset slider
        if(slider.value <= 0.01f)
        {
            canClick = true;
            handleImage.color = new Color(baseColor.r, baseColor.g, baseColor.b, 1f);
        }
    }

    private void AddToValue()
    {
        if(!canClick)
        {
            return;
        }

        canClick = false;

        //Grey out slider
        handleImage.color = new Color(baseColor.r, baseColor.g, baseColor.b, 0.4f);

        //Add value depending on bar value.
        float value = 0.5f;
        if(slider.value > 0.5f)
        {
            value = 1;
        }
        if(slider.value > 0.75f)
        {
            value = 2.5f;
        }
        doorCurrentValue += value;
    }
}
