using UnityEngine;

[RequireComponent(typeof(EnemyCollisionBehaviour))]
public class EnemyMovementBehaviour : BaseBehaviour
{

    EnemyEntityData data;

    public float moveSpeed;
    [SerializeField] UnityVoidEvent OnMovementStart, OnMovementEnd;

    bool isMoving;
    public bool IsMoving
    {
        get { return isMoving; }
        set
        {
            if (value != isMoving && value)
                OnMovementStart.Invoke();
            else if (value != isMoving)
                OnMovementEnd.Invoke();
            isMoving = value;
        }
    }

    Vector2 moveDirection;
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

    public void SetMoveDirection(Vector2 _moveDirection)
    {
        moveDirection = _moveDirection;
    }

    void CalculateVelocity()
    {
        velocity.x = moveDirection.x * moveSpeed;
        velocity.y += PlayerGameplayBehaviour.normalGravity * Time.deltaTime;
    }
}