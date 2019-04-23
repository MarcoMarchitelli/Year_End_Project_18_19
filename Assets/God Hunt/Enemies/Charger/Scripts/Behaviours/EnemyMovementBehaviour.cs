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
    [HideInInspector] public float currentMoveSpeed;

    protected override void CustomSetup()
    {
        data = Entity.Data as EnemyEntityData;
        currentMoveSpeed = moveSpeed;
    }

    public override void Enable(bool _value)
    {
        if (!_value)
            StopAllCoroutines();
        base.Enable(_value);
        print(_value);
    }

    public override void OnUpdate()
    {
        if (!IsSetupped)
            return;

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

    public void ResetMoveDirection()
    {
        moveDirection = Vector2.zero;
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
        velocity.x = moveDirection.x * currentMoveSpeed;
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
        float distance = Vector2.Distance(_target, transform.position);
        float pathTime = distance / _moveSpeed;
        float timer = 0;

        SetMoveDirection(moveDir);
        currentMoveSpeed = _moveSpeed;
        IsMoving = true;

        while (timer < pathTime)
        {
            timer += Time.deltaTime;
            data.enemyCollisionBehaviour.Move(moveDir * currentMoveSpeed * Time.deltaTime, false);
            yield return null;
        }

        currentMoveSpeed = moveSpeed;
        IsMoving = false;
        inRoutine = false;
        _callback?.Invoke();
    }
}