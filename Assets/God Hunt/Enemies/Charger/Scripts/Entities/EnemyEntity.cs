using UnityEngine;

public class EnemyEntity : BoxColliderEntity
{

    public override void CustomSetup()
    {
        Data = new EnemyEntityData(
            GetBehaviour<EnemyCollisionBehaviour>(),
            GetComponent<BoxCollider>()
            );
    }

}

public class EnemyEntityData : BoxColliderEntityData
{
    public EnemyCollisionBehaviour enemyCollisionBehaviour;

    public EnemyEntityData(EnemyCollisionBehaviour _ecb, BoxCollider _boxCollider)
    {
        collider = _boxCollider;
        enemyCollisionBehaviour = _ecb;
    }
}