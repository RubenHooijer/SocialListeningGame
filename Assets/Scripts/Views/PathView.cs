using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using Dialogue;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PathView : MonoBehaviour, IGuidable {

    private readonly static List<PathView> PathViews = new List<PathView>();

    [SerializeField][STRGuid] private string guid;
    [SerializeField] private BGCurve path;

    public string Guid => guid;
    public BGCurve Path => path;
    public BGCcTrs Trs { 
        get {
            if (trs == null) {
                trs = path.GetComponent<BGCcTrs>();
            }
            return trs;
        }
    }

    private BGCcTrs trs;

    public static PathView GetView(string guid) {
        return PathViews.Find(x => x.Guid.ToString() == guid);
    }

    private void OnEnable() {
        PathViews.Add(this);
    }

    private void OnDisable() {
        PathViews.Remove(this);
    }

}