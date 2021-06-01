using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class TriggerEvent : MonoBehaviour {

    [SerializeField] private UnityEvent TriggerEnterCallback;

    private void OnTriggerEnter(Collider other) {
        TriggerEnterCallback?.Invoke();
    }

}