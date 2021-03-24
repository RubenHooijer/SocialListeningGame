using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;

    public List<GameObject> Rooms;

    private void Awake()
    {
        Instance = this;
        CheckRooms();
    }

    //Check if room items contain room script, remove otherwise to prevent game breaking errors
    private void CheckRooms()
    {
        foreach(GameObject room in Rooms)
        {
            if (room.GetComponent<Room>() == null)
            {
                Rooms.Remove(room);
            }
        }
    }

    public void SpawnRoom(Vector3 currentRoomEndPoint)
    {
        //Get random room and script attached to it
        GameObject room = Rooms[Random.Range(0, Rooms.Count)];

        //Get room variables to spawn
        GameObject instantiatedRoom = Instantiate(room, new Vector3(0,100,0), Quaternion.identity);
        Room roomScript = instantiatedRoom.GetComponent<Room>();

        Vector3 roomPosition = new Vector3(0, 0, roomScript.GetFloorCenter().y + (roomScript.GetSize().z / 2)) + currentRoomEndPoint;
        Debug.Log(roomPosition);
        instantiatedRoom.transform.position = roomPosition;
    }
}
