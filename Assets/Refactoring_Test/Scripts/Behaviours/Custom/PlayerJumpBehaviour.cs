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
    /// <summary>
    /// Gravity multiplier applyed when falling.
    /// </summary>
    [SerializeField] float fallGravityMultiplier;

    #endregion

    float normalGravity;
    float fallGravity;
    float currentGravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    const int airJumps = 1;
    int airJumpsCount;

    PlayerEntityData data;

    protected override void CustomSetup()
    {
        data = Entity.Data as PlayerEntityData;
        normalGravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToReachJumpApex, 2);
        fallGravity = normalGravity * fallGravityMultiplier;
        currentGravity = normalGravity;
        maxJumpVelocity = Mathf.Abs(normalGravity) * timeToReachJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(normalGravity)) * minJumpHeight;
        Physics.gravity = new Vector2(0, currentGravity);
        airJumpsCount = 0;
    }

    #region API

    public void HandleJumpPress()
    {
        if (IsSetupped)
        {
            if (data.playerCollisionBehaviour.Below)
            {
                data.playerRB.velocity = new Vector2(0, maxJumpVelocity);
            }
            else if(airJumpsCount < 1)
            {
                data.playerRB.velocity = new Vector2(0, maxJumpVelocity);
                airJumpsCount++;
            }
        }
    }

    public void HandleJumpRelease()
    {
        if (data.playerRB.velocity.y > minJumpVelocity)
            data.playerRB.velocity = new Vector2(0, minJumpVelocity);
    }

    public void ResetAirJumpsCount()
    {
        airJumpsCount = 0;
    }

    public void ToggleFallingGravity(bool _value)
    {
        if (_value)
        {
            currentGravity = fallGravity;
        }
        else
        {
            currentGravity = normalGravity;
        }
        Physics.gravity = new Vector2(0, currentGravity);
    }

    #endregion

}