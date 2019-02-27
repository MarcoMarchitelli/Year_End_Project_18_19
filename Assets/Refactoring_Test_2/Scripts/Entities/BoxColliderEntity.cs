using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Refactoring
{
    public abstract class BoxColliderEntity : BaseEntity
    {
        [SerializeField] bool setupOnStart = false;

        public virtual void Start()
        {
            if (setupOnStart)
                SetUpEntity();
        }
    }

    public abstract class BoxColliderEntityData : IEntityData
    {
        public BoxCollider collider;
    }
}
