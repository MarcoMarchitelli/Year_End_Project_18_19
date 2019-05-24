using UnityEngine;

public class FollowerBehaviour : BaseBehaviour
{
    public float moveSpeed;
    public float turnSpeed;
    public bool startFollowingOnAwake = true;

    Transform target;
    float targetAngle, angle;
    Vector3 directionToTarget;

    bool follow = false;

    protected override void CustomSetup()
    {
        PlayerEntity player = FindObjectOfType<PlayerEntity>();
        if (player)
        {
            //player.OnDeath += EndFollow;
            target = player.transform;
        }

        if(startFollowingOnAwake)
            StartFollow();
    }

    void Update()
    {
        if (!follow)
            return;

        //go towards target
        directionToTarget = (target.position - transform.position).normalized;
        transform.Translate(-directionToTarget * moveSpeed * Time.deltaTime);

        //face target
        targetAngle = 90 - Mathf.Atan2(directionToTarget.z, directionToTarget.x) * Mathf.Rad2Deg;
        angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, Time.deltaTime * turnSpeed);
        transform.eulerAngles = Vector3.up * angle;
    }

    public void StartFollow()
    {
        follow = true;
    }

    public void EndFollow()
    {
        follow = false;
    }
}