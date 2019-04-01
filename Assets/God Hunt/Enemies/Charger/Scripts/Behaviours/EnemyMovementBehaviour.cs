using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyCollisionBehaviour))]
public class EnemyMovementBehaviour : BaseBehaviour
{

    EnemyEntityData data;

    Vector2 moveDirection;
    float moveSpeed;
    Vector2 velocity;

    protected override void CustomSetup()
    {
        data = Entity.Data as EnemyEntityData;
    }

    public override void OnUpdate()
    {
        CalculateVelocity();

        data.enemyCollisionBehaviour.Move(velocity * Time.deltaTime, false);

        if (data.enemyCollisionBehaviour.collisions.above || data.enemyCollisionBehaviour.Below)
        {
            velocity.y = 0;
        }
    }

    void CalculateVelocity()
    {
        velocity.x = moveDirection.x * moveSpeed;
        velocity.y += PlayerGameplayBehaviour.normalGravity * Time.deltaTime;
    }
}