using UnityEngine;
using System.Collections;

[RequireComponent(typeof(EnemyMovementBehaviour))]
public class EnemyPatrolBehaviour : BaseBehaviour
{
    EnemyEntityData data;

    #region References
    [SerializeField] Transform path;
    #endregion

    #region  Variants
    [SerializeField] bool rotatesToWaypoint = true;
    [SerializeField] bool patrolOnStart = true;
    #endregion

    #region Parameters
    [SerializeField] float waypointDistanceThreshold = .2f;
    [SerializeField] float waitTime = 1f;
    [SerializeField] float rotationAnglePerSecond = 90f;
    #endregion

    #region Events
    [SerializeField] UnityVoidEvent OnWaypointReached, OnPathFinished, OnMovementStart, OnMovementEnd, OnPatrolStart, OnPatrolEnd;
    #endregion

    protected override void CustomSetup()
    {
        data = Entity.Data as EnemyEntityData;

        if (!path)
        {
            Debug.LogWarning(name + " has no path referenced!");
            return;
        }
        if (patrolOnStart)
            StartPatrol();
    }

    public override void Enable(bool _value)
    {
        if (!_value)
            StopPatrol();
        base.Enable(_value);
    }

    #region API

    public void StartPatrol()
    {
        if (!IsSetupped)
        {
            Debug.LogWarning(name + "'s patrol behaviour is not setupped!");
            return;
        }
        if (path)
        {
            StartCoroutine(FollowPath());
            OnPatrolStart.Invoke();
        }
    }

    public void ContinuePatrol()
    {
        if (!IsSetupped)
        {
            Debug.LogWarning(name + "'s patrol behaviour is not setupped!");
            return;
        }
        data.enemyMovementBehaviour.TurnTo((nextPoint - data.graphics.position).normalized, rotationAnglePerSecond, RotationEndCallback);
    }

    public void StopPatrol()
    {
        if (!IsSetupped)
        {
            Debug.LogWarning(name + "'s patrol behaviour is not setupped!");
            return;
        }

        StopAllCoroutines();
        data.enemyMovementBehaviour.StopAllCoroutines();
        data.enemyMovementBehaviour.SetMoveDirection(Vector2.zero);
        wasInterrupted = true;
        OnPatrolEnd.Invoke();
    }

    public void ToggleRotationToWaypoint(bool _value)
    {
        if (!IsSetupped)
        {
            Debug.LogWarning(name + "'s patrol behaviour is not setupped!");
            return;
        }
        rotatesToWaypoint = _value;
    }

    #endregion

    #region Patrol mathods

    Vector3[] wayPoints;
    Vector3 nextPoint;
    int nextPointIndex;
    bool wasInterrupted = false;

    IEnumerator FollowPath()
    {
        if (!wasInterrupted)
        {
            wayPoints = new Vector3[path.childCount];

            for (int i = 0; i < wayPoints.Length; i++)
            {
                wayPoints[i] = path.GetChild(i).position;
                wayPoints[i] = new Vector3(wayPoints[i].x, wayPoints[i].y, wayPoints[i].z);
            }

            transform.position = wayPoints[0];
            nextPointIndex = 1;
            nextPoint = wayPoints[nextPointIndex];
        }

        while (true)
        {
            OnMovementStart.Invoke();
            yield return data.enemyMovementBehaviour.MoveTo(nextPoint, data.enemyMovementBehaviour.currentMoveSpeed);
            OnMovementEnd.Invoke();

            nextPointIndex = (nextPointIndex + 1) % wayPoints.Length;
            nextPoint = wayPoints[nextPointIndex];

            //events
            if (nextPointIndex == wayPoints.Length - 1)
                OnPathFinished.Invoke();
            else
                OnWaypointReached.Invoke();

            if (rotatesToWaypoint)
            {
                yield return data.enemyMovementBehaviour.TurnTo((nextPoint - data.graphics.position).normalized, rotationAnglePerSecond, RotationEndCallback);
            }
            else
            {
                data.enemyMovementBehaviour.ResetMoveDirection();
                data.enemyMovementBehaviour.ResetMoveSpeed();
                yield return new WaitForSeconds(waitTime);
            }

            yield return null;
        }
    }

    public void RotationEndCallback()
    {
        if (wasInterrupted)
        {
            StartCoroutine(FollowPath());
        }
    }

    #endregion

}