using UnityEngine;

public class CheckpointBehaviour : BaseBehaviour
{
    PlayerEntityData playerData;
    bool setupped;

    protected override void CustomSetup()
    {
        playerData = GameManager.Instance.player.Data as PlayerEntityData;
        setupped = true;
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
        if (!setupped)
            return;

        playerData.respawnBehaviour.SetRespawnPoint(_transform.position);
    }
}