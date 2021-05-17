using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationCylinder : MonoBehaviour
{
    public List<Transform> Cylinders;

    /// <summary>
    /// direction 1 = turn right, direction -1 = turn left
    /// </summary>
    /// <param name="cylinder"></param>
    /// <param name="direction"></param>
    public void RotateCylinder(int cylinder, int direction)
    {
        Cylinders[cylinder].Rotate(new Vector3(0,60 * direction, 0));
    }
}
