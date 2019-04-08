using UnityEngine;

public class EnemyEntity : BoxColliderEntity
{

    [SerializeField] Transform graphics;

    public override void CustomSetup()
    {
        Data = new EnemyEntityData(
            GetBehaviour<EnemyCollisionBehaviour>(),
            GetBehaviour<EnemyMovementBehaviour>(),
            graphics,
            GetComponent<BoxCollider>()
            );
    }

}

public class EnemyEntityData : BoxColliderEntityData
{
    public EnemyCollisionBehaviour enemyCollisionBehaviour;
    public EnemyMovementBehaviour enemyMovementBehaviour;
    public Transform graphics;

    public EnemyEntityData(EnemyCollisionBehaviour _ecb, EnemyMovementBehaviour _emb, Transform _g, BoxCollider _boxCollider)
    {
        collider = _boxCollider;
        enemyCollisionBehaviour = _ecb;
        enemyMovementBehaviour = _emb;
        graphics = _g;
    }
}