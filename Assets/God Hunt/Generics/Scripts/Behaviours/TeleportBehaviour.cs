using UnityEngine;

public class TeleportBehaviour : BaseBehaviour
{
    public Transform[] teleportPoints;
    public UnityVoidEvent OnTeleport;

    public void TeleportThisBehaviour(int _teleportPointIndex)
    {
        transform.position = teleportPoints[_teleportPointIndex].position;
        OnTeleport.Invoke();
    }

    public void TeleportThisEntity(int _teleportPointIndex)
    {
        Entity.gameObject.transform.position = teleportPoints[_teleportPointIndex].position;
        OnTeleport.Invoke();
    }
}