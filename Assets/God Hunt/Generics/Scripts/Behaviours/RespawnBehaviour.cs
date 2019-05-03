using UnityEngine;

public class RespawnBehaviour : BaseBehaviour
{
    PlayerEntityData data;

    [SerializeField] UnityVoidEvent OnDeathRespawn;
    [SerializeField] UnityVoidEvent OnCheckpointRespawn;

    Vector3 deathRespawnPoint;
    Vector3 checkPoint;

    protected override void CustomSetup()
    {
        SetRespawnPoint(Entity.gameObject.transform.position);
        data = Entity.Data as PlayerEntityData;
    }

    public void SetRespawnPoint(Vector3 _value)
    {
        deathRespawnPoint = _value;
    }

    public void SetCheckPoint(Vector3 _value)
    {
        checkPoint = _value;
    }

    public void Respawn(bool _isDeathRespawn)
    {
        if (_isDeathRespawn)
        {
            Entity.gameObject.transform.position = deathRespawnPoint;
            data.animatorProxy.IsDead = false;
            OnDeathRespawn.Invoke();
        }
        else
        {
            Entity.gameObject.transform.position = checkPoint;
            OnCheckpointRespawn.Invoke();
        }
    }
}