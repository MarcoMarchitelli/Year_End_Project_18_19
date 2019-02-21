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

    [SerializeField] UnityVoidEvent OnEntityRising;
    [SerializeField] UnityVoidEvent OnEntityFalling;

    #endregion

    Rigidbody rb;
    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;

    protected override void CustomSetup()
    {
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToReachJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToReachJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity)) * minJumpHeight;

        rb = Entity.gameObject.GetComponentInChildren<Rigidbody>();
        Physics.gravity = new Vector2(0, gravity);
    }

    #region API

    public void HandleJumpPress()
    {
        OnEntityRising.Invoke();
        rb.velocity = new Vector2(0, maxJumpVelocity);
    }

    public void HandleJumpRelease()
    {
        OnEntityFalling.Invoke();
        if (rb.velocity.y > minJumpVelocity)
            rb.velocity = new Vector2(0, minJumpVelocity);
    }

    #endregion

}