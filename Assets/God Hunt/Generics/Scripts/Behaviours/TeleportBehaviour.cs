using UnityEngine;

public class TeleportBehaviour : BaseBehaviour
{
    public Transform[] teleportPoints;
    public UnityVoidEvent OnTeleport;

    public void TeleportThisBehaviour(int _teleportPointIndex)
    {
        if (!isEnabled)
            return;

        transform.position = teleportPoints[_teleportPointIndex].position;
        OnTeleport.Invoke();
    }

    public void TeleportThisEntity(int _teleportPointIndex)
    {
        if (!isEnabled)
            return;

        Entity.gameObject.transform.position = teleportPoints[_teleportPointIndex].position;
        OnTeleport.Invoke();
    }
}