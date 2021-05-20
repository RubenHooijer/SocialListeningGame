using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotationCylinder : MonoBehaviour
{
    public List<CylinderWithRotation> Cylinders;

    private bool canRotate;

    [SerializeField] private float lerpDuration = 1;

    private void Start()
    {
        canRotate = true;
    }

    public void RotateCylinderUp(int cylinder)
    {
        if(canRotate)
        {
            StartCoroutine(LerpRotation2(60, cylinder));
            //Cylinders[cylinder].DOLocalRotate(new Vector3(Cylinders[cylinder].localRotation.x + 60, transform.rotation.y, transform.rotation.z), 1);
            StartCoroutine(RotationCooldown());
        }
    }

    public void RotateCylinderDown(int cylinder)
    {
        if (canRotate)
        {
            StartCoroutine(LerpRotation2(-60, cylinder));
            //Cylinders[cylinder].DOLocalRotate(new Vector3(Cylinders[cylinder].localRotation.x - 60, transform.rotation.y, transform.rotation.z), 1);
            StartCoroutine(RotationCooldown());
        }
    }

    private IEnumerator LerpRotation2(float rotation, int cylinder)
    {
        for(int i = 0; i < lerpDuration * 60; i++)
        {
            Cylinders[cylinder].Cylinder.Rotate(rotation / (60 * lerpDuration), 0,0);
            yield return new WaitForSeconds(lerpDuration / (60 * lerpDuration));
        }

        //Calculate current rotation
        Cylinders[cylinder].CurrentRotation += rotation;
        if (Cylinders[cylinder].CurrentRotation >= 180 || Cylinders[cylinder].CurrentRotation <= -180)
        {
            Cylinders[cylinder].CurrentRotation = 0;
        }

        yield return new WaitForSeconds(0.1f);
        CheckCorrectRotations();
    }

    private void CheckCorrectRotations()
    {
        int correctRotations = 0;

        Debug.Log(Cylinders[0].CurrentRotation);

        for (int i = 0; i < Cylinders.Count; i++)
        {
            if(Cylinders[i].CurrentRotation == Cylinders[i].CorrectRotation || Cylinders[i].CurrentRotation == (Cylinders[i].CorrectRotation - 180))
            {
                correctRotations++;
            }
        }
        Debug.Log(correctRotations);
        if(correctRotations > 4)
        {
            FinishMinigame();
        }
    }

    private void FinishMinigame()
    {

    }

    private IEnumerator RotationCooldown()
    {
        canRotate = false;
        yield return new WaitForSeconds(lerpDuration + 0.35f);
        canRotate = true;
    }
}

[System.Serializable]
public class CylinderWithRotation
{
    public Transform Cylinder;
    public float CorrectRotation;
    public float CurrentRotation = 0;
}
