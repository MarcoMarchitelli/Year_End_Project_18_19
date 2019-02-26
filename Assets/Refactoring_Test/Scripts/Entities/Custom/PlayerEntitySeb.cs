using UnityEngine;

namespace Refactoring
{
    public class PlayerEntitySeb : BoxColliderEntity
    {
        [SerializeField] bool setupOnStart = false;

        public override void Start()
        {
            base.Start();
        }

        public override void CustomSetup()
        {
            Data = new PlayerEntityData(GetBehaviour<Player>(), GetBehaviour<Controller3D>(), GetComponent<BoxCollider>());
        }
    }

    public class PlayerEntityData : BoxColliderEntityData
    {
        public Player player;
        public Controller3D controller3D;

        public PlayerEntityData(Player _p, Controller3D _c, BoxCollider _bc)
        {
            player = _p;
            controller3D = _c;
            collider = _bc;
        }
    } 
}