using UnityEngine;

public class PlayerRoomInteractionBehaviour : BaseBehaviour
{
    RoomSystem roomSystem;
    PlayerEntityData data;
    Room currentRoom;

    protected override void CustomSetup()
    {
        data = Entity.Data as PlayerEntityData;
        roomSystem = GameManager.Instance.roomSystem;
    }

    private void OnTriggerEnter(Collider other)
    {
        Room r = other.GetComponent<Room>();

        if (r && r!= currentRoom)
        {
            if(currentRoom)
                currentRoom.ToggleVCam(false);
            currentRoom = r;
            currentRoom.ToggleVCam(true);
            data.respawnBehaviour.SetCheckPoint(currentRoom.SpawnPoint.position);
            roomSystem.OnRoomEnter(r);
        }
    }
}