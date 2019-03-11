using UnityEngine;

public class RespawnBehaviour : BaseBehaviour
{
    [SerializeField] UnityVoidEvent OnRespawn;

    Vector3 respawnPoint;

    protected override void CustomSetup()
    {
        SetRespawnPoint(Entity.gameObject.transform.position);
    }

    public void SetRespawnPoint(Vector3 _value)
    {
        respawnPoint = _value;
    }

    public void Respawn()
    {
        Entity.gameObject.transform.position = respawnPoint;
        OnRespawn.Invoke();
    }

}