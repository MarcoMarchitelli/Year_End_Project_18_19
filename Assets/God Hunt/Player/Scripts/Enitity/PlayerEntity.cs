using UnityEngine;

public class PlayerEntity : BoxColliderEntity
{
    public override void CustomSetup()
    {
        Data = new PlayerEntityData(
            GetBehaviour<PlayerGameplayBehaviour>(),
            GetBehaviour<PlayerCollisionsBehaviour>(),
            GetBehaviour<PlayerInputBehaviour>(),
            GetBehaviour<PlayerAttacksBehaviour>(),
            GetComponent<BoxCollider>(),
            GetComponentInChildren<PlayerAnimatorProxy>(),
            GetBehaviour<DamageReceiverBehaviour>(),
            GetBehaviour<RespawnBehaviour>(),
            GetBehaviour<PlayerCameraTarget>()
            );
    }
}

public class PlayerEntityData : BoxColliderEntityData
{
    public PlayerGameplayBehaviour playerGameplayBehaviour;
    public PlayerCollisionsBehaviour playerCollisionsBehaviour;
    public PlayerInputBehaviour playerInputBehaviour;
    public PlayerAttacksBehaviour playerAttacksBehaviour;
    public PlayerAnimatorProxy animatorProxy;
    public DamageReceiverBehaviour damageReceiverBehaviour;
    public RespawnBehaviour respawnBehaviour;
    public PlayerCameraTarget cameraTarget;

    public PlayerEntityData(PlayerGameplayBehaviour _p, PlayerCollisionsBehaviour _c, PlayerInputBehaviour _pib, PlayerAttacksBehaviour _pab, 
        BoxCollider _bc, PlayerAnimatorProxy _ap, DamageReceiverBehaviour _drb, RespawnBehaviour _rb, PlayerCameraTarget _pct)
    {
        playerGameplayBehaviour = _p;
        playerCollisionsBehaviour = _c;
        playerInputBehaviour = _pib;
        playerAttacksBehaviour = _pab;
        collider = _bc;
        animatorProxy = _ap;
        damageReceiverBehaviour = _drb;
        respawnBehaviour = _rb;
        cameraTarget = _pct;
    }
}