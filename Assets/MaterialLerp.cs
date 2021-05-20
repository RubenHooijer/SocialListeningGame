using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialLerp : MonoBehaviour
{
    public Renderer Oorwurm;
    public Material teeth;
    public Material noTeeth;
    public float duration;

    public bool showTeeth;
    // Start is called before the first frame update
    void Start()
    {
        
        Oorwurm.material = noTeeth;
        showTeeth = false;
    }


public void lerpMaterial()
    {
        //showTeeth = true;
        Oorwurm.material.Lerp (noTeeth, teeth, 1);
    }

    void Update()
    {
        if (showTeeth)
        {
            

        }
    }


}
