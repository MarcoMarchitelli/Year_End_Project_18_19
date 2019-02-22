using UnityEngine;

public class PlayerEntity : BaseEntity
{
    [SerializeField] bool setupOnStart = false;

    void Start()
    {
        if(setupOnStart)
            SetUpEntity();
    }

    public override void CustomSetup()
    {
        Data = new PlayerEntityData(GetBehaviour<PlayerCollisionBehaviour>(), GetBehaviour<PlayerMovementBehaviour>(), GetBehaviour<PlayerInputBehaviour>());
    }
}

public class PlayerEntityData : IEntityData
{
    public PlayerCollisionBehaviour playerCollisionBehaviour;
    public PlayerMovementBehaviour playerMovementBehaviour;
    public PlayerInputBehaviour playerInputBehaviour;

    public PlayerEntityData(PlayerCollisionBehaviour _pcb, PlayerMovementBehaviour _pmb, PlayerInputBehaviour _pib)
    {
        playerCollisionBehaviour = _pcb;
        playerMovementBehaviour = _pmb;
        playerInputBehaviour = _pib;
    }
}