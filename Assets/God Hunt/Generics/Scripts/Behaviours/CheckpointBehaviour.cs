using UnityEngine;

public class CheckpointBehaviour : BaseBehaviour
{
    PlayerEntityData playerData;

    protected override void CustomSetup()
    {
        playerData = GameManager.Instance.player.Data as PlayerEntityData;
    }

    /// <summary>
    /// Sets the player's respawn point as this object position.
    /// </summary>
    public void SetCheckpointAsThis()
    {
        playerData.respawnBehaviour.SetRespawnPoint(transform.position);
    }

    public void SetCheckpoint(Transform _transform)
    {
        playerData.respawnBehaviour.SetRespawnPoint(_transform.position);
    }
}