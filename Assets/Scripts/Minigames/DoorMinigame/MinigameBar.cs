using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameBar : MonoBehaviour
{
    private Slider slider;

    [SerializeField] private float speed = 1f;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    private void Update()
    {   
        slider.value = Mathf.PingPong(Time.time, 1);
    }
}
