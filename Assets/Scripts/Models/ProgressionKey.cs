using System;
using UnityEngine;

[Serializable]
public class ProgressionKey {

    [SerializeField] private string guid;

    public bool IsEmpty => string.IsNullOrEmpty(guid);
    public string Guid => guid;

    public string Label {
        get {
            if (ProgressionKeys.Instance.HasKey(guid)) {
                return ProgressionKeys.Instance.GetKey(guid).Label;
            } else {
                return guid;
            }
        }
    }

    public ProgressionKey(string guid) {
        this.guid = guid;
    }

    public override int GetHashCode() {
        if (string.IsNullOrEmpty(guid)) {
            return 0;
        }

        return guid.GetHashCode();
    }

    public override bool Equals(object obj) {
        if (obj is ProgressionKey == false) {
            return false;
        }

        ProgressionKey other = (ProgressionKey)obj;
        return other.guid == guid;
    }

    public static bool operator ==(ProgressionKey obj1, ProgressionKey obj2) {
        if (ReferenceEquals(obj1, obj2)) {
            return true;
        }

        if (ReferenceEquals(obj1, null)) {
            return false;
        }
        if (ReferenceEquals(obj2, null)) {
            return false;
        }

        return obj1.guid == obj2.guid;
    }

    public static bool operator !=(ProgressionKey obj1, ProgressionKey obj2) {
        if (ReferenceEquals(obj1, obj2)) {
            return false;
        }

        if (ReferenceEquals(obj1, null)) {
            return true;
        }
        if (ReferenceEquals(obj2, null)) {
            return true;
        }

        return obj1.guid != obj2.guid;
    }

}