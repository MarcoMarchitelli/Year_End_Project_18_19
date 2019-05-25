using UnityEngine;

public class PlayerKnockbackReceiverBehaviour : KnockbackReceiverBehaviour
{
    PlayerEntityData data;

    protected override void CustomSetup()
    {
        data = Entity.Data as PlayerEntityData;
    }

    public override void KnockbackMove(Vector2 _direction, float _speed)
    {
        if (!IsSetupped)
            return;

        data.playerCollisionsBehaviour.Move(_direction * _speed * Time.deltaTime, false);
    }
}