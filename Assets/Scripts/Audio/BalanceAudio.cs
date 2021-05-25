using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using FMODUnityResonance;

public class BalanceAudio : MonoBehaviour
{
    public static BalanceAudio Instance;

    [FMODUnity.EventRef]
    EventInstance eventInstance;
    public float paramValue;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        paramValue = 0.5f;
        eventInstance = RuntimeManager.CreateInstance("event:/Balance game plateau");
        //eventInstance.getParameterByName("plateau_tilt_speed", out paramValue);
        eventInstance.setParameterByName("plateau_tilt_speed", paramValue);
        PlayBalanceAudio();
    }

    public void PlayBalanceAudio()
    {
        eventInstance.start();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            paramValue += 0.05f;
            eventInstance.setParameterByName("plateau_tilt_speed", paramValue);
        }
    }
}
