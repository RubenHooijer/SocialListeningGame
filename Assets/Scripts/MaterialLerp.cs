using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialLerp : MonoBehaviour
{
    public Renderer Oorwurm;
    public bool showTeeth;
    private float lerp;
    private float t = 0.0f;
    public float speed;
    public GameObject particles;

    void Start()
    {

        Oorwurm = Oorwurm.GetComponent<Renderer>();
        showTeeth = false;
        particles.SetActive(false);
    }

    public void smokeExplosion()
    {
        particles.SetActive(true);
    }
    public void StartMat()
    {
        showTeeth = true;
    }

    void Update()
    {
        if (t <= 1 && showTeeth)
        {
            lerp = Mathf.Lerp(0, 1, t);
            t += speed * Time.deltaTime;
            Oorwurm.material.SetFloat("_Tand", lerp);
        }
        
    }


}
