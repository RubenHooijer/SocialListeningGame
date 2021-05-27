using System.Collections.Generic;
using UnityEngine;

public class StaticView : MonoBehaviour, IGuidable {

    private readonly static List<StaticView> Views = new List<StaticView>();

    [SerializeField] [STRGuid] private string guid;
    public Renderer Renderer;

    public string Guid => guid;

    public static StaticView GetView(string guid) {
        return Views.Find(x => x.Guid == guid);
    }

    private void OnEnable() {
        Views.Add(this);
    }

    private void OnDisable() {
        Views.Remove(this);
    }

}