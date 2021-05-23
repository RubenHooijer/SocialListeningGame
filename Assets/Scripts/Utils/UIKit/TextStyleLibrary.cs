using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "TextStyleLibrary", menuName = "TextMeshPro/Style Library")]
public class TextStyleLibrary : ScriptableObject {
    
    [Serializable]
    public class Style {
        [Space]
        public string Name;
        public TMP_FontAsset Font;
        public Material Material;
        public FontStyles FontStyle;
        public Color Color;
        [Space]
        public int Size;
        public bool AutoSize;
        public int SizeMin;
        public int SizeMax;
        [Space]
        public bool WordWrapping;
        public int LineSpacing;
        public bool IsNewGroup;
    }

    [SerializeField] public List<Style> Styles = new List<Style>();

}