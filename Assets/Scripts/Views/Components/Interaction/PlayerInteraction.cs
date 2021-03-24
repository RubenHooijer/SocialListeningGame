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
            Quaternion.identity,
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
        Gizmos.DrawWireCube((transform.forward * forwardMultiplier) + boxOrigin + transform.position, boxSize);
        style.normal.textColor = Color.green;
        style.fontSize = 16;
        UnityEditor.Handles.Label((transform.forward * forwardMultiplier) + boxOrigin + transform.position, "Interactable cast", style);

    }
#endif
}