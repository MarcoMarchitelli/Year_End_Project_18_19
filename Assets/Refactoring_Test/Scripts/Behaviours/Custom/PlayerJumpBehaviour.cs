using UnityEngine;

public class PlayerJumpBehaviour : BaseBehaviour
{

    #region Serialized Fields

    /// <summary>
    /// Max height the jumping entity can reach, relative to itself.
    /// </summary>
    [SerializeField] float maxJumpHeight;
    /// <summary>
    /// Min height the jumping entity can reach, relative to itself.
    /// </summary>
    [SerializeField] float minJumpHeight;
    /// <summary>
    /// Time in which the jumping entity reaches the max jump height.
    /// </summary>
    [SerializeField] float timeToReachJumpApex;

    #endregion

    Rigidbody rb;
    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    const int airJumps = 1;
    int airJumpsCount;

    PlayerEntityData data;

    protected override void CustomSetup()
    {
        rb = Entity.gameObject.GetComponentInChildren<Rigidbody>();

        data = Entity.Data as PlayerEntityData;
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToReachJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToReachJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity)) * minJumpHeight;
        Physics.gravity = new Vector2(0, gravity);
        airJumpsCount = 0;
    }

    #region API

    public void HandleJumpPress()
    {
        if (IsSetupped)
        {
            if (data.playerCollisionBehaviour.Below)
            {
                rb.velocity = new Vector2(0, maxJumpVelocity);
            }
            else if(airJumpsCount < 1)
            {
                rb.velocity = new Vector2(0, maxJumpVelocity);
                airJumpsCount++;
            }
        }
    }

    public void HandleJumpRelease()
    {
        if (rb.velocity.y > minJumpVelocity)
            rb.velocity = new Vector2(0, minJumpVelocity);
    }

    public void ResetAirJumpsCount()
    {
        airJumpsCount = 0;
    }

    #endregion

}