using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
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