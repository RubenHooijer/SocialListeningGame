using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallToMinigameTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        BalanceMinigame.Instance.LoadScene(3);
    }
}
