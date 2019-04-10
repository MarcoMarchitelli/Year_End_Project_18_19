using UnityEngine;

public class RoomSystem : MonoBehaviour
{
    [SerializeField] Room startingRoom;
    Room firstRoom, secondRoom;

    public void Setup()
    {
        Room[] rooms = FindObjectsOfType<Room>();
        for (int i = 0; i < rooms.Length; i++)
        {
            rooms[i].Setup();
        }
        firstRoom = startingRoom;
        firstRoom.Discover();
        PlayerEntityData playerData = GameManager.Instance.player.Data as PlayerEntityData;
        playerData.respawnBehaviour.SetRespawnPoint(firstRoom.SpawnPoint.position);
    }

    public void OnRoomEnter(Room _room)
    {
        if (_room.Unclosable)
            return;
        else
        if (_room != firstRoom && !secondRoom)
        {
            secondRoom = _room;
            secondRoom.Discover();
        }
        else
        if (_room != firstRoom & _room != secondRoom)
        {
            firstRoom.Close();
            firstRoom = secondRoom;
            secondRoom = _room;
            secondRoom.Discover();
        }
    }
}