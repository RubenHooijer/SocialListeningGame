using System.Collections.Generic;
using System.Runtime.Serialization;

[DataContract]
public class Progression {

    [DataMember]
    private HashSet<ProgressionKey> keys = new HashSet<ProgressionKey>();

    public IEnumerable<ProgressionKey> Keys => keys;

    public void SetKey(ProgressionKey key) {
        if (keys.Contains(key) || key.IsEmpty) { return; }

        keys.Add(key);
    }

    public void RemoveKey(ProgressionKey key) {
        if (!keys.Contains(key) || key.IsEmpty) { return; }

        keys.Remove(key);
    }

    public bool HasKey(ProgressionKey key) {
        if (key.IsEmpty) { return true; }

        return keys.Contains(key);
    }

}
