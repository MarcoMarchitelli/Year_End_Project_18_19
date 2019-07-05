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

    public void SetCurrentRoom(Room _room)
    {
        currentRoom = _room;
        CameraManager.Instance.ChangeActiveCam(currentRoom.vCam);
        data.respawnBehaviour.SetCheckPoint(currentRoom.SpawnPoint.position);
        roomSystem.OnRoomEnter(currentRoom);
        if (currentRoom.tag == "Boss")
            data.inBossRoom = true;
        else
            data.inBossRoom = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isEnabled)
            return;

        Room r = other.GetComponent<Room>();

        if (r && r!= currentRoom)
        {
            SetCurrentRoom(r);
        }
    }
}