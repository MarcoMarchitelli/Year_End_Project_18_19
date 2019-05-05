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
        dirToTarget = (target.position - transform.position).normalized;
        data.enemyMovementBehaviour.TurnTo(dirToTarget, turnToTargetSpeed, StartCharge);
    }

    /// <summary>
    /// Triggers an event which is responsible for animation triggering and knockback settings.
    /// </summary>
    public void StartCharge()
    {
        OnChargeStart.Invoke();
    }

    /// <summary>
    /// Interrupts the charge and calls related event.
    /// </summary>
    public void StopCharge()
    {
        data.enemyMovementBehaviour.ResetMoveDirection();
        data.enemyMovementBehaviour.ResetMoveSpeed();
        data.enemyMovementBehaviour.StopAllCoroutines();
        OnChargeEnd.Invoke(0.1f);
    }

    /// <summary>
    /// Performs the charge.
    /// </summary>
    public void Charge()
    {
        data.enemyMovementBehaviour.MoveTo((Vector2)transform.position + new Vector2(Mathf.Sign( dirToTarget.x ) * chargeDistance, 0), chargeSpeed, EndCharge);
    }

    #endregion

    /// <summary>
    /// Calls an event which handles the end of the charge (animations, cooldown and knockback settings).
    /// </summary>
    void EndCharge()
    {
        OnChargeEnd.Invoke(chargeCooldown);
    }
}
