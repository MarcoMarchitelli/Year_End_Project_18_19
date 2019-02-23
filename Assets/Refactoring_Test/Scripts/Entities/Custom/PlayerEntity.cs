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
        Data = new PlayerEntityData(GetComponent<Rigidbody>(), GetBehaviour<PlayerCollisionBehaviour>(), GetBehaviour<PlayerMovementBehaviour>(), GetBehaviour<PlayerInputBehaviour>(), GetBehaviour<PlayerJumpBehaviour>(), GetBehaviour<PlayerDashBehaviour>());
    }
}

public class PlayerEntityData : IEntityData
{
    public Rigidbody playerRB;
    public PlayerCollisionBehaviour playerCollisionBehaviour;
    public PlayerMovementBehaviour playerMovementBehaviour;
    public PlayerInputBehaviour playerInputBehaviour;
    public PlayerJumpBehaviour playerJumpBehaviour;
    public PlayerDashBehaviour playerDashBehaviour;

    public PlayerEntityData(Rigidbody _rb, PlayerCollisionBehaviour _pcb, PlayerMovementBehaviour _pmb, PlayerInputBehaviour _pib, PlayerJumpBehaviour _pjb, PlayerDashBehaviour _pdb)
    {
        playerRB = _rb;
        playerCollisionBehaviour = _pcb;
        playerMovementBehaviour = _pmb;
        playerInputBehaviour = _pib;
        playerJumpBehaviour = _pjb;
        playerDashBehaviour = _pdb;
    }
}