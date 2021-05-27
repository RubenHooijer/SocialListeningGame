using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimationWithOffset : MonoBehaviour
{

    [SerializeField] public string animationName;
    private Animator[] animators;

    private void Awake()
    {
        animators = GetComponentsInChildren<Animator>();

        for (int i = 0; i < animators.Length; i++)
        {
            animators[i].Play(animationName, 0, Random.value);
        }
    }

}
