using System.Collections.Generic;
using UnityEngine;

public class AudioClipInstanceLimiter : MonoBehaviour {

    public int MaxInstances { get { return maxInstances; } }

    public int CurrentInstanceAmount {
        get {
            if (runningInstanceCount.ContainsKey(soundID) == false) {
                return 0;
            }
            return runningInstanceCount[soundID];
        }
    }

    private static Dictionary<string, int> runningInstanceCount = new Dictionary<string, int>();

    [SerializeField] private string soundID = "";
    [SerializeField] private int maxInstances = 1;

    private bool isAdded;

    public void AddInstance() {
        if (isAdded) { return; }

        if (runningInstanceCount.ContainsKey(soundID) == false) {
            runningInstanceCount.Add(soundID, 0);
        }

        runningInstanceCount[soundID]++;

        isAdded = true;
    }

    public void RemoveInstance() {
        if (!isAdded) { return; }

        runningInstanceCount[soundID]--;
        isAdded = false;
    }

    private void OnDestroy() {
        RemoveInstance();
    }

    private void OnValidate() {
        if (string.IsNullOrEmpty(soundID)) {
            soundID = gameObject.name;
        }
    }

}