using UnityEngine;

public class PlayerEntity : BoxColliderEntity
{
    public override void Start()
    {
        base.Start();
    }

    public override void CustomSetup()
    {
        Data = new PlayerEntityData(
            GetBehaviour<PlayerGameplayBehaviour>(),
            GetBehaviour<PlayerCollisionsBehaviour>(),
            GetBehaviour<PlayerInputBehaviour>(),
            GetComponent<BoxCollider>(),
            GetComponentInChildren<AnimatorProxy>(),
            GetBehaviour<DamageReceiverBehaviour>(),
            GetBehaviour<RespawnBehaviour>()
            );
    }
}

public class PlayerEntityData : BoxColliderEntityData
{
    public PlayerGameplayBehaviour playerGameplayBehaviour;
    public PlayerCollisionsBehaviour playerCollisionsBehaviour;
    public PlayerInputBehaviour playerInputBehaviour;
    public AnimatorProxy animatorProxy;
    public DamageReceiverBehaviour damageReceiverBehaviour;
    public RespawnBehaviour respawnBehaviour;

    public PlayerEntityData(PlayerGameplayBehaviour _p, PlayerCollisionsBehaviour _c, PlayerInputBehaviour _pib, BoxCollider _bc, AnimatorProxy _ap, DamageReceiverBehaviour _drb, RespawnBehaviour _rb)
    {
        playerGameplayBehaviour = _p;
        playerCollisionsBehaviour = _c;
        playerInputBehaviour = _pib;
        collider = _bc;
        animatorProxy = _ap;
        damageReceiverBehaviour = _drb;
        respawnBehaviour = _rb;
    }
}