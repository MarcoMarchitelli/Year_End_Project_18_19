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
    [SerializeField] UnityVoidEvent OnWaypointReached, OnPathFinished;
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
        }
    }

    public void ContinuePatrol()
    {
        if (!IsSetupped)
        {
            Debug.LogWarning(name + "'s patrol behaviour is not setupped!");
            return;
        }
        StartCoroutine(RotateTo());
    }

    public void StopPatrol()
    {
        if (!IsSetupped)
        {
            Debug.LogWarning(name + "'s patrol behaviour is not setupped!");
            return;
        }

        StopAllCoroutines();
        data.enemyMovementBehaviour.SetMoveDirection(Vector2.zero);
        wasInterrupted = true;
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
            data.enemyMovementBehaviour.SetMoveDirection(((Vector2)nextPoint - (Vector2)transform.position).normalized);
            data.enemyMovementBehaviour.IsMoving = true;

            if (Vector2.Distance(transform.position, nextPoint) <= waypointDistanceThreshold)
            {
                data.enemyMovementBehaviour.SetMoveDirection(Vector2.zero);
                nextPointIndex = (nextPointIndex + 1) % wayPoints.Length;
                nextPoint = wayPoints[nextPointIndex];
                data.enemyMovementBehaviour.IsMoving = false;

                //events
                if (nextPointIndex == wayPoints.Length - 1)
                    OnPathFinished.Invoke();
                else
                    OnWaypointReached.Invoke();

                if (rotatesToWaypoint)
                {
                    yield return StartCoroutine(RotateTo());
                }
                else
                    yield return new WaitForSeconds(waitTime);
            }
            yield return null;
        }
    }

    IEnumerator RotateTo()
    {
        Vector3 directionToTarget = (nextPoint - data.graphics.position).normalized;
        float targetAngle = Mathf.Atan2(directionToTarget.z, directionToTarget.x) * Mathf.Rad2Deg;

        while (Mathf.DeltaAngle(data.graphics.eulerAngles.y, targetAngle) > 0.05f || Mathf.DeltaAngle(data.graphics.eulerAngles.y, targetAngle) < -0.05f)
        {
            float angle = Mathf.MoveTowardsAngle(data.graphics.eulerAngles.y, targetAngle, Time.deltaTime * rotationAnglePerSecond);
            data.graphics.eulerAngles = Vector3.up * angle;
            yield return null;
        }

        if (wasInterrupted)
            StartCoroutine(FollowPath());
    }

    #endregion

}