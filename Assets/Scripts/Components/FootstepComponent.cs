using UnityEngine;

public class FootstepComponent : MonoBehaviour {

    [SerializeField] private GameObject leftFoot;
    [SerializeField] private GameObject rightFoot;
    [SerializeField] private GameObject jump;

    public void LeftFootstep() {
        leftFoot.SetActive(false);
        leftFoot.SetActive(true);
    }

    public void RightFootstep() {
        rightFoot.SetActive(false);
        rightFoot.SetActive(true);
    }

    public void Jump() {
        jump.SetActive(false);
        jump.SetActive(true);
    }

}