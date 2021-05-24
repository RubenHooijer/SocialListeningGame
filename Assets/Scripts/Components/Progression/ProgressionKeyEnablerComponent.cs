using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionKeyEnablerComponent : MonoBehaviour {

    [SerializeField] private ProgressionKey enableOnKey;

    private void Start() {
        gameObject.SetActive(IsProgressionKeySet());
        Eventbus.ProgressionKeyChangedEvent.AddListener(OnProgressionKeyChanged);
    }

    private void OnDestroy() {
        Eventbus.ProgressionKeyChangedEvent.RemoveListener(OnProgressionKeyChanged);
    }

    private void OnProgressionKeyChanged(ProgressionKey progressionKey, bool isSet) {
        gameObject.SetActive(IsProgressionKeySet());
    }

    private bool IsProgressionKeySet() {
        return !enableOnKey.IsEmpty && World.Instance.Progression.HasKey(enableOnKey);
    }

}