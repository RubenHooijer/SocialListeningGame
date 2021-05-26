using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPuzzleMinigame : MonoBehaviour
{
    [SerializeField] private GameObject GameUI, GameCylinder;

    [SerializeField] private Animator CinemachineAnimator;

    public void StartMinigame()
    {
        StartCoroutine(MoveToCylinder());
    }

    private IEnumerator MoveToCylinder()
    {
        FadeScript.Instance.Fade(1, 1);
        yield return new WaitForSeconds(1);
        CinemachineAnimator.SetBool("CylinderCam", true);
        yield return new WaitForSeconds(0.5f);
        FadeScript.Instance.Fade(0, 1);
        GameUI.SetActive(true);
        GameCylinder.SetActive(true);
    }

    //REMOVE UPDATE || TESTING ONLY
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            StartMinigame();
        }
    }
}
