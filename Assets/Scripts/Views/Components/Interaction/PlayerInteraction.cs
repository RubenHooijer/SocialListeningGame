using UnityEngine;

public class PlayerInteraction : MonoBehaviour {

    [Header("Settings")]
    [SerializeField] private Vector3 boxOrigin;
    [SerializeField] private Vector3 boxSize;
    [SerializeField] private float forwardMultiplier;
    [SerializeField] private LayerMask interactableMask;

    private IInteractable focusedInteractable;

    private void OnEnable() {
        InputManager.Instance.InteractPerformed.AddListener(OnInteractPerformed);
    }

    private void OnDisable() {
        InputManager.Instance.InteractPerformed.RemoveListener(OnInteractPerformed);
    }

    private void Update() {
        CheckForInteractable();
    }

    private void OnInteractPerformed() {
        if (focusedInteractable == null) { return; }
        focusedInteractable.Interact();
    }

    private void CheckForInteractable() {
        Collider[] colliders = Physics.OverlapBox(
            (transform.forward * forwardMultiplier) + boxOrigin + transform.position, 
            boxSize, 
            transform.rotation,
            interactableMask);

        if (colliders.Length < 1) {
            focusedInteractable = null;
        }

        for (int i = 0; i < colliders.Length; i++) {
            Collider collider = colliders[i];

            if (collider.TryGetComponent(out IInteractable interactable)) {
                focusedInteractable = interactable;
                break;
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        GUIStyle style = new GUIStyle();

        Gizmos.color = Color.green;
        Matrix4x4 prevMatrix = Gizmos.matrix;
        Gizmos.matrix = transform.localToWorldMatrix;

        Vector3 boxPosition = (transform.forward * forwardMultiplier) + boxOrigin + transform.position;
        Vector3 inversedBoxPosition = transform.InverseTransformPoint(boxPosition);

        Gizmos.DrawWireCube(inversedBoxPosition, boxSize * 2f);
        style.normal.textColor = Color.green;
        style.fontSize = 16;
        UnityEditor.Handles.Label(boxPosition, "Interactable cast", style);

        Gizmos.matrix = prevMatrix;

    }
#endif
}