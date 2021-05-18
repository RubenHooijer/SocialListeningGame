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
        yield return new WaitForSeconds(0.1f);
        CheckCorrectRotations();
    }

    private void CheckCorrectRotations()
    {
        int correctRotations = 0;

        float angle = Cylinders[0].Cylinder.localEulerAngles.x;
        //angle = (angle > 180) ? angle - 360 : angle;
        //angle = Mathf.Round(angle);
        Debug.Log(angle + ",  " + Cylinders[0].Cylinder.name);

        for (int i = 0; i < Cylinders.Count; i++)
        {
            //Calculate angle in correct way
            //float angle = Cylinders[i].Cylinder.localEulerAngles.x;
            //angle = (angle > 180) ? angle - 360 : angle;
            //angle = Mathf.Round(angle);

            //Debug.Log(angle);

            if(angle == Cylinders[i].Rotation || angle == (Cylinders[i].Rotation - 180))
            {
                correctRotations++;
            }
        }
        //Debug.Log(correctRotations);
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
    public float Rotation;
}
