using UnityEngine;

public class FootstepComponent : MonoBehaviour {

    [SerializeField] private GameObject leftFoot;
    [SerializeField] private GameObject rightFoot;
    [SerializeField] private GameObject jump;

    public void LeftFootstep() {
        if (leftFoot == null) { return; }
        leftFoot.SetActive(false);
        leftFoot.SetActive(true);
    }

    public void RightFootstep() {
        if (rightFoot == null) { return; }
        rightFoot.SetActive(false);
        rightFoot.SetActive(true);
    }

    public void Jump() {
        if (jump == null) { return; }
        jump.SetActive(false);
        jump.SetActive(true);
    }

}