using UnityEngine;

public class RespawnBehaviour : BaseBehaviour
{
    [SerializeField] UnityVoidEvent OnDeathRespawn;
    [SerializeField] UnityVoidEvent OnCheckpointRespawn;

    Vector3 respawnPoint;

    protected override void CustomSetup()
    {
        SetRespawnPoint(Entity.gameObject.transform.position);
    }

    public void SetRespawnPoint(Vector3 _value)
    {
        respawnPoint = _value;
    }

    public void Respawn(bool _isDeathRespawn)
    {
        if (_isDeathRespawn)
        {
            Entity.gameObject.transform.position = respawnPoint;
            OnDeathRespawn.Invoke();
        }
        else
        {
            Entity.gameObject.transform.position = respawnPoint;
            OnCheckpointRespawn.Invoke();
        }
    }

}