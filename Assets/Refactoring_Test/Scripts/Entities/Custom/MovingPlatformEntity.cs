using UnityEngine;

namespace Refactoring
{
    public class MovingPlatformEntity : BoxColliderEntity
    {
        [SerializeField] bool setupOnStart = false;

        public override void Start()
        {
            base.Start();
        }

        public override void CustomSetup()
        {
            Data = new MovingPlatformEntityData(GetComponent<BoxCollider>());
        }
    }

    public class MovingPlatformEntityData : BoxColliderEntityData
    {
        public MovingPlatformEntityData(BoxCollider _bc)
        {
            collider = _bc;
        }
    }
}