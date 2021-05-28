using System.Collections.Generic;
using UnityEngine;

public class CylinderView : MonoBehaviour, IGuidable {

    private readonly static List<CylinderView> Views = new List<CylinderView>();

    [SerializeField] [STRGuid] private string guid;
    public Renderer Renderer;

    public string Guid => guid;

    public static CylinderView GetView(string guid) {
        return Views.Find(x => x.Guid == guid);
    }

    private void OnEnable() {
        Views.Add(this);
    }

    private void OnDisable() {
        Views.Remove(this);
    }

}