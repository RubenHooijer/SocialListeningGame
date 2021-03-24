using Oasez.Extensions.Generics.Singleton;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : GenericSingleton<RoomManager, RoomManager>
{

    [SerializeField] private List<Room> Rooms;

    public void SpawnRoom(Vector3 currentRoomEndPoint)
    {
        //Get random room and script attached to it
        Room randomRoom = Rooms[Random.Range(0, Rooms.Count)];

        //Get room variables to spawn
        Room room = Instantiate(randomRoom, new Vector3(0,100,0), Quaternion.identity);

        Vector3 roomPosition = new Vector3(0, 0, room.GetFloorCenter().y + (room.GetSize().z / 2)) + currentRoomEndPoint;
        Debug.Log(roomPosition);
        room.transform.position = roomPosition;
    }

}