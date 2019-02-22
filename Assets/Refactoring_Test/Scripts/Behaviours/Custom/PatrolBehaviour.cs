using UnityEngine;
using System.Collections;

public class PatrolBehaviour : BaseBehaviour
{
    #region References
    [SerializeField] Transform path;
    #endregion

    #region  Variants
    [SerializeField] bool rotatesToWaypoint = true;
    [SerializeField] bool patrolOnStart = true;
    #endregion

    #region Parameters
    [SerializeField] float speed = 5f;
    [SerializeField] float waitTime = 1f;
    [SerializeField] float rotationAnglePerSecond = 90f;
    #endregion

    #region Events
    [SerializeField] UnityVoidEvent OnMovementStart, OnMovementEnd, OnWaypointReached, OnPathFinished;
    #endregion

    #region Properties

    bool isMoving;

    public bool IsMoving
    {
        get { return isMoving; }
        private set
        {
            if (value != isMoving && value)
                OnMovementStart.Invoke();
            else if (value != isMoving)
                OnMovementEnd.Invoke();
            isMoving = value;
        }
    }

    #endregion

    protected override void CustomSetup()
    {
        if (!path)
        {
            Debug.LogWarning(name + " has no path referenced!");
            return;
        }
        if (patrolOnStart)
            StartPatrol();
    }

    #region API

    public void StartPatrol()
    {
        StartCoroutine(FollowPath());
    }

    public void StopPatrol()
    {
        StopCoroutine(FollowPath());
    }

    public void ToggleRotationToWaypoint(bool _value)
    {
        rotatesToWaypoint = _value;
    }

    #endregion

    #region Patrol mathods

    IEnumerator FollowPath()
    {
        Vector3[] wayPoints = new Vector3[path.childCount];

        for (int i = 0; i < wayPoints.Length; i++)
        {
            wayPoints[i] = path.GetChild(i).position;
            wayPoints[i] = new Vector3(wayPoints[i].x, wayPoints[i].y, wayPoints[i].z);
        }

        transform.position = wayPoints[0];
        int nextPointIndex = 1;
        Vector3 nextPoint = wayPoints[nextPointIndex];
        transform.LookAt(nextPoint);

        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, nextPoint, speed * Time.deltaTime);
            IsMoving = true;
            if (transform.position == nextPoint)
            {
                nextPointIndex = (nextPointIndex + 1) % wayPoints.Length;
                nextPoint = wayPoints[nextPointIndex];
                IsMoving = false;

                //events
                if (nextPointIndex == wayPoints.Length - 1)
                    OnPathFinished.Invoke();
                else
                    OnWaypointReached.Invoke();

                if (rotatesToWaypoint)
                    yield return StartCoroutine(RotateTo(nextPoint));
                else
                    yield return new WaitForSeconds(waitTime);
            }
            yield return null;
        }
    }

    IEnumerator RotateTo(Vector3 _rotationTarget)
    {
        Vector3 directionToTarget = (_rotationTarget - transform.position).normalized;
        float targetAngle = 90 - Mathf.Atan2(directionToTarget.z, directionToTarget.x) * Mathf.Rad2Deg;

        while (Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle) > 0.05f || Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle) < -0.05f)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, Time.deltaTime * rotationAnglePerSecond);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        }
    }

    #endregion

}