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
        yield return new WaitForSeconds(1.5f);

        FadeScript.Instance.Fade(1,3);
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
