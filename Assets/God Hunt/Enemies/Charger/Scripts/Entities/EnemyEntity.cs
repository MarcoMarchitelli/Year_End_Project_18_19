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
            GetComponent<BoxCollider>(),
            GetBehaviour<EnemyPatrolBehaviour>()
            );
    }

}

public class EnemyEntityData : BoxColliderEntityData
{
    public EnemyCollisionBehaviour enemyCollisionBehaviour;
    public EnemyMovementBehaviour enemyMovementBehaviour;
    public Transform graphics;
    public EnemyPatrolBehaviour enemyPatrolBehaviour;

    public EnemyEntityData(EnemyCollisionBehaviour _ecb, EnemyMovementBehaviour _emb, Transform _g, BoxCollider _boxCollider, EnemyPatrolBehaviour _enemyPatrolBehaviour)
    {
        collider = _boxCollider;
        enemyCollisionBehaviour = _ecb;
        enemyMovementBehaviour = _emb;
        graphics = _g;
        enemyPatrolBehaviour = _enemyPatrolBehaviour;
    }
}