
using UnityEngine;

public class EarGrowOnKey : MonoBehaviour
{

    public Animator anim;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            anim.SetBool("Grow", true);
            anim.speed = Random.RandomRange(0.4f, 1.0f);
        }
    }
}