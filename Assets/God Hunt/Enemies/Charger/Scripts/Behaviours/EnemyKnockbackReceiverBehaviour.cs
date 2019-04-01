using UnityEngine;

public class EnemyKnockbackReceiverBehaviour : KnockbackReceiverBehaviour
{
    EnemyEntityData data;

    protected override void CustomSetup()
    {
        data = Entity.Data as EnemyEntityData;
    }

    public override void KnockbackMove(Vector2 _direction, float _speed)
    {
        data.enemyCollisionBehaviour.Move(_direction * _speed * Time.deltaTime, false);
    }
}