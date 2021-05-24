using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public float speed = 5f;

    // Update is called once per frame
    void Start()
    {
        transform.DORotate(new Vector3(0, 360, 0), speed, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);

    }
}