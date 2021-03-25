using UnityEngine;

public class Door : MonoBehaviour, IInteractable {

    private RoomManager roomManager;
    private Room room;
    private bool Opened;

    private void Start()
    {
        roomManager = RoomManager.Instance;
        room = GetComponentInParent<Room>();
    }

    public void Interact() {
        if (!CanDoorBeOpened()) { return; }

        roomManager.SpawnRoom(room.GetEndPoint());
        Open();
    }

    private void Open() {
        Opened = true;
        gameObject.SetActive(false);
    }

    private bool CanDoorBeOpened() {
        return true;
    }

}