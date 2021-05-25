using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rock : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" || other.tag == "Eustachius")
        {
            StartCoroutine(PlayerDead());
        }
    }

    private IEnumerator PlayerDead()
    {
        //Particle Effect

        FadeScript.Instance.Fade(1,1);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
