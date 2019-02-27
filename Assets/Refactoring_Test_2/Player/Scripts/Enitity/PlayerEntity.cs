using UnityEngine;

namespace Refactoring
{
    public class PlayerEntity : BoxColliderEntity
    {
        public override void Start()
        {
            base.Start();
        }

        public override void CustomSetup()
        {
            Data = new PlayerEntityData(GetBehaviour<PlayerGameplayBehaviour>(), GetBehaviour<PlayerCollisionsBehaviour>(), GetComponent<BoxCollider>(), GetComponentInChildren<AnimatorProxy>());
        }
    }

    public class PlayerEntityData : BoxColliderEntityData
    {
        public PlayerGameplayBehaviour playerGameplayBehaviour;
        public PlayerCollisionsBehaviour controller3D;
        public AnimatorProxy animatorProxy;

        public PlayerEntityData(PlayerGameplayBehaviour _p, PlayerCollisionsBehaviour _c, BoxCollider _bc, AnimatorProxy _ap)
        {
            playerGameplayBehaviour = _p;
            controller3D = _c;
            collider = _bc;
            animatorProxy = _ap;
        }
    } 
}