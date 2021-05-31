using DG.Tweening;
using UnityEngine;

public class MaterialLerp : MonoBehaviour
{
    public Renderer Oorwurm;
    public GameObject particles;
    [SerializeField] private float teethShowDuration = 1;

    void Start()
    {
        Oorwurm = Oorwurm.GetComponent<Renderer>();
        particles.SetActive(false);
    }

    public void smokeExplosion()
    {
        particles.SetActive(true);
    }

    public void ShowTeeth()
    {
        float startValue = Oorwurm.material.GetFloat("_Tand");
        float endValue = 1;
        float value = startValue;
        DOTween.To(() => value, x => value = x, endValue, teethShowDuration)
            .OnUpdate(() => Oorwurm.material.SetFloat("_Tand", value));
    }

    public void HideTeeth()
    {
        float startValue = Oorwurm.material.GetFloat("_Tand");
        float endValue = 0;
        float value = startValue;
        DOTween.To(() => value, x => value = x, endValue, teethShowDuration)
            .OnUpdate(() => Oorwurm.material.SetFloat("_Tand", value));
    }

}
