using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTransformComponent : MonoBehaviour {

    public static Transform Transform { get; private set; }

    private void Awake() {
        Transform = transform;
    }

}