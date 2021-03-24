using UnityEngine;

public abstract class ContainerItem<T> : MonoBehaviour {

    public T Data { get; private set; }

    public void Setup(T data) {
        gameObject.SetActive(true);
        Data = data;
        OnSetup(data);
    }

    public void Dispose() {
        OnDispose();
    }

    protected virtual void OnSetup(T data) { }
    
    protected virtual void OnDispose() { }

}