using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerCollisionsBehaviour))]
public class PlayerGameplayBehaviour : BaseBehaviour
{
    PlayerEntityData data;

    #region Serialized Fields

    [Header("Movement")]
    [SerializeField] float moveSpeed = 6;

    //TODO: for now we hide this and have it at 0 cuz of air dash momentum issue.
    [Tooltip("Time it takes to the player to linearly interpolate from no speed to move speed, while grounded")]
    float accelerationTimeGrounded = 0f;
    //TODO: for now we hide this and have it at 0 cuz of air dash momentum issue.
    [Tooltip("Time it takes to the player to linearly interpolate from no speed to move speed, while in air")]
    float accelerationTimeAirborne = 0;

    [Header("Sprinting")]
    [Tooltip("Behaviour's running speed.")]
    [SerializeField] float runMoveSpeed;
    [Tooltip("Time the entity takes to reach the end of the run acceleration curve.")]
    [SerializeField] float accelerationTime;
    [Tooltip("Describes how the move speed interpolates to run speed.")]
    [SerializeField] AnimationCurve runAccelerationCurve;
    [Tooltip("Time the entity takes to reach the end of the run deceleration curve.")]
    [SerializeField] float decelerationTime;
    [Tooltip("Describes how the run speed interpolates back to move speed.")]
    [SerializeField] AnimationCurve runDecelerationCurve;
    [SerializeField] UnityVoidEvent OnAccelerationStart, OnAccelerationEnd, OnDecelerationStart, OnDecelerationEnd;

    [Header("Dashing")]
    [SerializeField] float minDashDistance = 5f;
    [SerializeField] float maxDashDistance = 7f;
    [SerializeField] float minDashTime;
    [SerializeField] float dashCooldown;
    [SerializeField] UnityVoidEvent OnDashStart;
    [SerializeField] UnityFloatEvent OnDashEnd;

    [Header("Jumping")]
    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    public float doubleJumpHeight = 2;
    public float timeToJumpApex = .4f;
    public float fallingGravityMultiplier = 2f;
    [SerializeField] UnityVoidEvent OnDoubleJump;

    #endregion

    #region Vars

    #region Jumping

    [HideInInspector] public bool canDoubleJump;
    [HideInInspector] public bool hasDoubleJumped = false;
    int jumpsCount;

    #endregion

    #region Wall Jumping

    [HideInInspector]
    public Vector2 wallJumpClimb;
    [HideInInspector]
    public Vector2 wallJumpOff;
    [HideInInspector]
    public Vector2 wallLeap;
    [HideInInspector]
    public float wallSlideSpeedMax = 3;
    [HideInInspector]
    public float wallStickTime = .25f;
    float timeToWallUnstick;
    bool wallSliding;
    int wallDirX;

    #endregion

    #region Velocity

    float currentMoveSpeed;
    public static float normalGravity;
    float fallingGravity;
    float currentGravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    float doubleJumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;

    #endregion

    #region Sprinting

    [HideInInspector] public bool accelerating;
    float accelTimer;
    bool decelerating;
    float decelTimer;
    bool runReleasedInAir;
    bool groundCollisionDecelHandled;

    #endregion

    #region Dashing

    bool dashPressed;
    Vector3 dashDirection;
    float dashSpeed;
    float maxDashTime;
    const int airDashes = 1;
    int airDashesCount;
    bool _isDashing = false;
    bool IsDashing
    {
        get { return _isDashing; }
        set
        {
            if (_isDashing != value)
            {
                _isDashing = value;
                data.animatorProxy.IsDashing = _isDashing;
                if (!_isDashing)
                {
                    OnDashEnd.Invoke(dashCooldown);
                }
            }
        }
    }

    #endregion

    #region Faceing

    int _faceingDirection;
    int FaceingDirection
    {
        get { return _faceingDirection; }
        set
        {
            if (_faceingDirection != value)
            {
                _faceingDirection = value;
                if (_faceingDirection > 0)
                    data.animatorProxy.transform.rotation = Quaternion.Euler(rightFaceingDirection);
                else if (_faceingDirection < 0)
                    data.animatorProxy.transform.rotation = Quaternion.Euler(leftFaceingDirection);
            }
        }
    }
    Vector3 rightFaceingDirection = Vector3.zero;
    Vector3 leftFaceingDirection = Vector3.up * 180;

    #endregion

    Vector2 directionalInput;

    #endregion

    #region Overrides

    protected override void CustomSetup()
    {
        data = Entity.Data as PlayerEntityData;

        #region Jumping
        normalGravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        fallingGravity = normalGravity * fallingGravityMultiplier;
        maxJumpVelocity = Mathf.Abs(normalGravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(normalGravity) * minJumpHeight);
        doubleJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(normalGravity) * doubleJumpHeight);
        jumpsCount = 0;
        #endregion

        #region Movement
        currentMoveSpeed = moveSpeed;
        accelerationTime = Mathf.Abs(accelerationTime);
        decelerationTime = Mathf.Abs(decelerationTime);
        #endregion

        #region Dashing
        FaceingDirection = 1;
        dashSpeed = minDashDistance / minDashTime;
        maxDashTime = maxDashDistance / dashSpeed;
        airDashesCount = 0;
        #endregion
    }

    public override void OnUpdate()
    {
        #region Calculations

        HandleSprinting();
        SetGravity();
        CalculateVelocity();
        //HandleWallSliding();

        #endregion

        #region Movement

        data.playerCollisionsBehaviour.Move(velocity * Time.deltaTime, directionalInput);

        #endregion

        #region Collisions

        if (data.playerCollisionsBehaviour.collisions.above || data.playerCollisionsBehaviour.Below)
        {
            velocity.y = 0;
        }

        #endregion

        #region Animations

        HandleAnimations();

        #endregion

    }

    #endregion

    #region API

    public void ToggleDoubleJump(bool _value)
    {
        canDoubleJump = _value;
    }

    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;
    }

    public void OnJumpInputDown()
    {
        //if (wallSliding)
        //{
        //    if (wallDirX == directionalInput.x)
        //    {
        //        velocity.x = -wallDirX * wallJumpClimb.x;
        //        velocity.y = wallJumpClimb.y;
        //    }
        //    else if (directionalInput.x == 0)
        //    {
        //        velocity.x = -wallDirX * wallJumpOff.x;
        //        velocity.y = wallJumpOff.y;
        //    }
        //    else
        //    {
        //        velocity.x = -wallDirX * wallLeap.x;
        //        velocity.y = wallLeap.y;
        //    }
        //}
        if (data.playerCollisionsBehaviour.Below)
        {
            velocity.y = maxJumpVelocity;
            jumpsCount++;
        }
        else if (canDoubleJump && jumpsCount < 2 && !hasDoubleJumped)
        {
            velocity.y = doubleJumpVelocity;
            jumpsCount++;
            OnDoubleJump.Invoke();
            hasDoubleJumped = true;
        }
    }

    public void OnJumpInputUp()
    {
        if (velocity.y > minJumpVelocity)
        {
            velocity.y = minJumpVelocity;
        }
    }

    public void HandleDashPress()
    {
        if (isEnabled && !IsDashing)
        {
            if (data.playerCollisionsBehaviour.Below)
                StartDash();
            else if (airDashesCount < 1)
                StartDash(true);
        }
    }

    public void HandleDashRelease()
    {
        if (!IsDashing)
            return;

        dashPressed = false;
    }

    public void HandleSprintPress()
    {
        if (data.playerCollisionsBehaviour.Below)
        {
            accelerating = true;
            decelerating = false;
            decelTimer = 0;
            OnAccelerationStart.Invoke();
        }
    }

    public void HandleSprintRelease()
    {
        //Reset accel timer.
        accelerating = false;
        accelTimer = 0;

        OnDecelerationStart.Invoke();

        //check ground collision
        if (data.playerCollisionsBehaviour.Below)
        {
            decelerating = true;
            runReleasedInAir = false;
        }
        else
        {
            runReleasedInAir = true;
            groundCollisionDecelHandled = false;
        }

    }

    /// <summary>
    /// Checks needed to see wether we have momentum to take in account when touching the ground.
    /// </summary>
    /// <param name="_value"></param>
    public void HandleGroundCollision()
    {
        if (runReleasedInAir && !groundCollisionDecelHandled)
        {
            decelerating = true;
            decelTimer = 0;
            groundCollisionDecelHandled = true;
        }

        airDashesCount = 0;
        jumpsCount = 0;
        hasDoubleJumped = false;
    }

    #endregion

    #region Player Gameplay Methods

    void StartDash(bool _isAirDash = false)
    {
        if (_isAirDash)
            airDashesCount++;

        StopAllCoroutines();
        StartCoroutine(DashRoutine());

        OnDashStart.Invoke();
    }

    IEnumerator DashRoutine()
    {
        dashPressed = true;
        IsDashing = true;

        float currentDashTime = minDashTime;
        float timer = 0;

        while(true)
        {
            timer += Time.deltaTime;

            if (dashPressed)
            {
                if (timer > maxDashTime)
                    break;
            }
            else
            {
                if (timer > minDashTime)
                    break;
            }

            yield return null;
        }

        IsDashing = false;

    }

    void HandleWallSliding()
    {
        wallDirX = (data.playerCollisionsBehaviour.collisions.left) ? -1 : 1;
        wallSliding = false;
        if ((data.playerCollisionsBehaviour.collisions.left || data.playerCollisionsBehaviour.collisions.right) && !data.playerCollisionsBehaviour.Below && velocity.y < 0)
        {
            wallSliding = true;

            if (velocity.y < -wallSlideSpeedMax)
            {
                velocity.y = -wallSlideSpeedMax;
            }

            if (timeToWallUnstick > 0)
            {
                velocityXSmoothing = 0;
                velocity.x = 0;

                if (directionalInput.x != wallDirX && directionalInput.x != 0)
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else
            {
                timeToWallUnstick = wallStickTime;
            }

        }

    }

    void HandleSprinting()
    {
        if (accelerating)
        {
            accelTimer += Time.deltaTime;
            if (accelTimer > accelerationTime)
                accelTimer = accelerationTime;
            EvaluateSprintCurve(accelerating);
        }

        if (decelerating)
        {
            decelTimer += Time.deltaTime;
            if (decelTimer > decelerationTime)
                decelTimer = decelerationTime;
            EvaluateSprintCurve(accelerating);
        }
    }

    float interpolationValue;
    void EvaluateSprintCurve(bool _isAccelerating)
    {
        if (_isAccelerating)
        {
            if (accelerationTime != 0)
            {
                interpolationValue = runAccelerationCurve.Evaluate(accelTimer / accelerationTime);
                currentMoveSpeed = Mathf.Lerp(moveSpeed, runMoveSpeed, interpolationValue);
                if(interpolationValue == 1)
                {
                    OnAccelerationEnd.Invoke();
                }
            }
            else
            {
                currentMoveSpeed = runMoveSpeed;
            }
        }
        else
        {
            if (decelerationTime != 0)
            {
                interpolationValue = runDecelerationCurve.Evaluate(decelTimer / decelerationTime);
                currentMoveSpeed = Mathf.Lerp(moveSpeed, runMoveSpeed, interpolationValue);
                if (interpolationValue == 0)
                {
                    OnDecelerationEnd.Invoke();
                }
            }
            else
            {
                currentMoveSpeed = moveSpeed;
            }
        }
    }

    void SetGravity()
    {
        if (velocity.y < 0)
        {
            currentGravity = fallingGravity;
        }
        else
        {
            currentGravity = normalGravity;
        }
    }

    void CalculateVelocity()
    {
        if (!IsDashing)
        {
            float targetVelocityX = directionalInput.x * currentMoveSpeed;
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (data.playerCollisionsBehaviour.Below) ? accelerationTimeGrounded : accelerationTimeAirborne);
            velocity.y += currentGravity * Time.deltaTime;
        }
        else
        {
            velocity.x = dashSpeed * FaceingDirection;
            velocity.y = 0;
        }
    }

    void HandleAnimations()
    {
        if (data.playerCollisionsBehaviour.Below)
            data.animatorProxy.IsGrounded = true;
        else
            data.animatorProxy.IsGrounded = false;

        if (velocity.x > 0)
        {
            FaceingDirection = 1;
            if (data.animatorProxy.IsGrounded)
                data.animatorProxy.IsWalking = true;
            else
                data.animatorProxy.IsWalking = false;
        }
        else if (velocity.x < 0)
        {
            FaceingDirection = -1;
            if (data.animatorProxy.IsGrounded)
                data.animatorProxy.IsWalking = true;
            else
                data.animatorProxy.IsWalking = false;
        }
        else if (data.animatorProxy.IsGrounded)
            data.animatorProxy.IsWalking = false;

        if (velocity.y > 0)
            data.animatorProxy.IsRising = true;
        else if (velocity.y < 0)
            data.animatorProxy.IsRising = false;
    }

    #endregion
}