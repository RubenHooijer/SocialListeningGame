using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FallToMinigameTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        BalanceMinigame.Instance.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
