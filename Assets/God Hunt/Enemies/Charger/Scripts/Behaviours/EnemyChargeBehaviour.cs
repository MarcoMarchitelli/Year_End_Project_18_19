using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChargeBehaviour : BaseBehaviour
{
    EnemyEntityData data;

    [SerializeField] float chargeSpeed;
    [SerializeField] float turnToTargetSpeed = 90;

    Transform target;

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
        data.enemyPatrolBehaviour.StopPatrol();
        data.enemyMovementBehaviour.TurnTo((transform.position - target.position).normalized, turnToTargetSpeed, Charge);
    }

    #endregion

    void Charge()
    {
        data.enemyMovementBehaviour.moveSpeed = chargeSpeed;
    }
}
