using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private InputManager inputManager;
    private RoomManager roomManager;

    private Room room;

    private bool Opened;

    private void Start()
    {
        inputManager = InputManager.Instance;
        roomManager = RoomManager.Instance;
        room = GetComponentInParent<Room>();
    }

    private void OnTriggerStay(Collider other)
    {
        //TODO: Fix inputs not registering all like they should
        if(inputManager.GetInteract())
        {
            if(!CheckVariables())
            {
                return;
            }

            roomManager.SpawnRoom(room.GetEndPoint(), room.transform.position);

            //Make sure we dont spawn multiple rooms on top of each other
            Opened = true;

            //TODO: Door animation
            gameObject.SetActive(false);
        }      
    }

    private bool CheckVariables()
    {
        //Check if all variables are right to move on to the next room
        return true;
    }
}
