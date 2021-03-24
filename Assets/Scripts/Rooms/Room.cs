using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private Transform floor, endPoint;

    private Vector2 floorCenter;

    private Vector3 roomSize;

    // Start is called before the first frame update
    private void Awake()
    {
        floorCenter = new Vector2((floor.localScale.x * 10) / 2, (floor.localScale.z * 10) / 2);
        roomSize = new Vector3(floor.localScale.x * 10, 1, floor.localScale.z * 10);
        Debug.Log(floorCenter);
    }

    public Vector2 GetFloorCenter()
    {
        return floorCenter;
    }

    public Vector2 GetEndPoint()
    {
        return endPoint.position;
    }

    public Vector3 GetSize()
    {
        return roomSize;
    }
}
