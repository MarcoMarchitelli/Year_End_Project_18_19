using UnityEngine;

namespace Refactoring
{
    public class PlatformEntity : BoxColliderEntity
    {
        public override void Start()
        {
            base.Start();
        }

        public override void CustomSetup()
        {
            Data = new PlatformEntityData(GetComponent<BoxCollider>());
        }
    }

    public class PlatformEntityData : BoxColliderEntityData
    {
        public PlatformEntityData(BoxCollider _bc)
        {
            collider = _bc;
        }
    }
}