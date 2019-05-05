using UnityEngine;

[RequireComponent(typeof(EnemyMovementBehaviour))]
public class EnemyChargeBehaviour : BaseBehaviour
{
    EnemyEntityData data;

    [SerializeField] float chargeSpeed;
    [SerializeField] float chargeDistance;
    [SerializeField] float turnToTargetSpeed = 90;
    [SerializeField] float chargeCooldown = 2;
    [SerializeField] UnityVoidEvent OnChargeStart;
    [SerializeField] UnityFloatEvent OnChargeEnd;

    Transform target;
    Vector3 dirToTarget;

    protected override void CustomSetup()
    {
        data = Entity.Data as EnemyEntityData;
    }

    #region API

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    public void StartCharge()
    {
        OnChargeStart.Invoke();
        dirToTarget = (target.position - transform.position).normalized;
        data.enemyMovementBehaviour.TurnTo(dirToTarget, turnToTargetSpeed, Charge);
    }

    public void StopCharge()
    {
        data.enemyMovementBehaviour.ResetMoveDirection();
        data.enemyMovementBehaviour.ResetMoveSpeed();
        data.enemyMovementBehaviour.StopAllCoroutines();
        OnChargeEnd.Invoke(0.1f);
    }

    #endregion

    void Charge()
    {
        data.enemyMovementBehaviour.MoveTo((Vector2)transform.position + new Vector2(Mathf.Sign( dirToTarget.x ) * chargeDistance, 0), chargeSpeed, EndCharge);
    }

    void EndCharge()
    {
        OnChargeEnd.Invoke(chargeCooldown);
    }
}
