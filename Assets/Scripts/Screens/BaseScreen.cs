using UnityEngine;

public abstract class BaseScreen : MonoBehaviour {

    public void Show() {
        OnShow();
    }

    public void Hide() {
        OnHide();
    }

    protected virtual void OnShow() { }
    protected virtual void OnHide() { }

}