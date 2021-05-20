using UnityEngine;

public class ScaleToFit : MonoBehaviour {

    private void OnEnable() {
        UpdateScale();
    }

    [ContextMenu("Update Scale")]
    private void UpdateScale() {
        transform.localScale = Vector3.one;
        Vector2 mySize = (transform as RectTransform).rect.size;
        float myRatio = mySize.x / mySize.y;

        Vector2 parentSize = (transform.parent as RectTransform).rect.size;
        float parentRatio = parentSize.x / parentSize.y;
        
        if (myRatio > parentRatio) {
            transform.localScale = Vector3.one * parentSize.x / mySize.x;
        } else {
            transform.localScale = Vector3.one * parentSize.y / mySize.y;
        }
    }

}