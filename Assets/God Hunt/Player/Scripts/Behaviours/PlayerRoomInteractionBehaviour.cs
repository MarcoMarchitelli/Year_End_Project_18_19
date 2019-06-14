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
        if (!isEnabled)
            return;

        Room r = other.GetComponent<Room>();

        if (r && r!= currentRoom)
        {
            currentRoom = r;
            CameraManager.Instance.ChangeActiveCam(currentRoom.vCam);
            data.respawnBehaviour.SetCheckPoint(currentRoom.SpawnPoint.position);
            roomSystem.OnRoomEnter(r);
        }
    }
}