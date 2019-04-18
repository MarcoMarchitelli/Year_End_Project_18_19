using UnityEngine;
using System.Collections;
using System;

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
    bool inRoutine = false;

    protected override void CustomSetup()
    {
        data = Entity.Data as EnemyEntityData;
    }

    public override void Enable(bool _value)
    {
        if (!_value)
            StopAllCoroutines();
        base.Enable(_value);
    }

    public override void OnUpdate()
    {
        if (!inRoutine)
        {
            CalculateVelocity();
            data.enemyCollisionBehaviour.Move(velocity * Time.deltaTime, false);
        }

        if (data.enemyCollisionBehaviour.collisions.above || data.enemyCollisionBehaviour.Below)
        {
            velocity.y = 0;
        }
    }

    public void SetMoveDirection(Vector2 _moveDirection)
    {
        moveDirection = _moveDirection;
    }

    public Coroutine TurnTo(Vector3 _dir, float _rotationAnglePerSecond, Action _callback = null)
    {
        StopAllCoroutines();
        return StartCoroutine(RotateTo(_dir, _rotationAnglePerSecond, _callback));
    }

    public Coroutine MoveTo(Vector3 _target, float _moveSpeed, Action _callback = null)
    {
        StopAllCoroutines();
        return StartCoroutine(MoveToTarget(_target, _moveSpeed, _callback));
    }

    void CalculateVelocity()
    {
        velocity.x = moveDirection.x * moveSpeed;
        velocity.y += PlayerGameplayBehaviour.normalGravity * Time.deltaTime;
    }

    IEnumerator RotateTo(Vector3 _dir, float _rotationAnglePerSecond, Action _callback = null)
    {
        inRoutine = true;
        float targetAngle = Mathf.Atan2(_dir.z, _dir.x) * Mathf.Rad2Deg;

        while (Mathf.DeltaAngle(data.graphics.eulerAngles.y, targetAngle) > 0.05f || Mathf.DeltaAngle(data.graphics.eulerAngles.y, targetAngle) < -0.05f)
        {
            float angle = Mathf.MoveTowardsAngle(data.graphics.eulerAngles.y, targetAngle, Time.deltaTime * _rotationAnglePerSecond);
            data.graphics.eulerAngles = Vector3.up * angle;
            yield return null;
        }

        _callback?.Invoke();
        inRoutine = false;
    }

    IEnumerator MoveToTarget(Vector3 _target, float _moveSpeed, Action _callback = null)
    {
        inRoutine = true;
        velocity = Vector2.zero;
        Vector3 moveDir = (_target - transform.position).normalized;
        SetMoveDirection(moveDir);
        moveSpeed = _moveSpeed;
        IsMoving = true;

        while (Vector2.Distance(transform.position, _target) > .1f)
        {
            data.enemyCollisionBehaviour.Move(moveDir * moveSpeed * Time.deltaTime, false);
            yield return null;
        }

        _callback?.Invoke();
        IsMoving = false;
        inRoutine = false;
    }
}