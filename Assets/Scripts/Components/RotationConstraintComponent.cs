using UnityEngine;

public class RotationConstraintComponent : MonoBehaviour {

    [SerializeField] private bool lockX = false;
    [SerializeField] private bool lockY = false;
    [SerializeField] private bool lockZ = false;

    private Vector3 startRotation;

    private void Awake() {
        startRotation = transform.eulerAngles;
    }

    private void LateUpdate() {
        Vector3 currentRotation = transform.eulerAngles;
        Vector3 newRotation = new Vector3(
            lockX ? startRotation.x : currentRotation.x,
            lockY ? startRotation.y : currentRotation.y,
            lockZ ? startRotation.z : currentRotation.z);

        transform.eulerAngles = newRotation;
    }

}