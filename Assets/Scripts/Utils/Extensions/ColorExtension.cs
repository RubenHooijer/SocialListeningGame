using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class ColorExtension {
    public static bool EqualsAlmost(this Color color, Color other) {
        return color.Equals(other);
    }
}
