using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public abstract class BoxColliderEntity : BaseEntity
{

}

public abstract class BoxColliderEntityData : IEntityData
{
    public BoxCollider collider;
}