using UnityEngine;

public class PlayerMovementBehaviour : BaseBehaviour
{

    #region Serialized Fields

    /// <summary>
    /// Behaviour's movement speed.
    /// </summary>
    [Tooltip("Behaviour's movement speed.")]
    [SerializeField]
    float moveSpeed;
    /// <summary>
    /// Behaviour's movement smoothing time when not running.
    /// </summary>
    [Tooltip("Behaviour's movement smoothing time when not running.")]
    [SerializeField]
    float moveSmoothingTime = .1f;
    /// <summary>
    /// Behaviour's running speed.
    /// </summary>
    [Tooltip("Behaviour's running speed.")]
    [SerializeField]
    float runMoveSpeed;
    /// <summary>
    /// Time the entity takes to reach the end of the run acceleration curve.
    /// </summary>
    [Tooltip("Time the entity takes to reach the end of the run acceleration curve.")]
    [SerializeField]
    float accelerationTime;
    /// <summary>
    /// Describes how the move speed interpolates to run speed.
    /// </summary>
    [Tooltip("Describes how the move speed interpolates to run speed.")]
    [SerializeField]
    AnimationCurve runAccelerationCurve;
    /// <summary>
    /// Time the entity takes to reach the end of the run deceleration curve.
    /// </summary>
    [Tooltip("Time the entity takes to reach the end of the run deceleration curve.")]
    [SerializeField]
    float decelerationTime;
    /// <summary>
    /// Describes how the run speed interpolates back to move speed.
    /// </summary>
    [Tooltip("Describes how the run speed interpolates back to move speed.")]
    [SerializeField]
    AnimationCurve runDecelerationCurve;
    /// <summary>
    /// Evento lanciato all'inzio del movimento
    /// </summary>
    [SerializeField] UnityVoidEvent OnMovementStart;
    /// <summary>
    /// Evento lanciato alla fine del movimento
    /// </summary>
    [SerializeField] UnityVoidEvent OnMovementStop;
    /// <summary>
    /// Event called when entity rigidbody velocity y is increasing.
    /// </summary>
    [SerializeField] UnityVoidEvent OnEntityRising;
    /// <summary>
    /// Event called when entity rigidbody velocity y is decreading.
    /// </summary>
    [SerializeField] UnityVoidEvent OnEntityFalling;

    #endregion

    #region Variables

    #region References

    PlayerEntityData data;

    #endregion

    #region Move

    Vector3 moveVelocity;
    Vector2 moveDirection;
    float currentMoveSpeed;
    float moveVelocityX;
    float targetMoveVelocityX;
    float velocityXSmoothing;

    #endregion

    #region Run

    bool accelerating;
    float accelTimer;
    bool decelerating;
    float decelTimer;
    bool runReleasedInAir;
    bool groundCollisionDecelHandled;

    #endregion

    bool _isRising;
    bool IsRising
    {
        get { return _isRising; }
        set
        {
            if (value != _isRising)
            {
                _isRising = value;
                if (_isRising)
                {
                    OnEntityRising.Invoke();
                    data.playerJumpBehaviour.ToggleFallingGravity(false);
                }
                else
                {
                    OnEntityFalling.Invoke();
                    data.playerJumpBehaviour.ToggleFallingGravity(true);
                }
            }
        }
    }

    #endregion

    protected override void CustomSetup()
    {
        data = Entity.Data as PlayerEntityData;
        //WTFF
        data.playerDashBehaviour.SetDashDirection(Vector3.right);
        //---
        currentMoveSpeed = moveSpeed;
    }

    public override void OnFixedUpdate()
    {
        if (IsSetupped)
        {
            Move();
            FaceMoveDirection();
        }
    }

    public override void OnUpdate()
    {
        if (accelerating)
        {
            accelTimer += Time.deltaTime;
            if (accelTimer > accelerationTime)
                accelTimer = accelerationTime;
            EvaluateRunCurve(accelerating);
        }

        if (decelerating)
        {
            decelTimer += Time.deltaTime;
            if (decelTimer > decelerationTime)
                decelTimer = decelerationTime;
            EvaluateRunCurve(accelerating);
        }

        CheckMoveVelocityX();
        CheckMoveVelocityY();
    }

    #region Behaviour's Methods

    /// <summary>
    /// Makes the player face right or left based on move direction.
    /// </summary>
    void FaceMoveDirection()
    {
        if (moveVelocityX > 0)
        {
            transform.rotation = Quaternion.Euler(Vector3.zero);
            data.playerDashBehaviour.SetDashDirection(Vector3.right);
        }
        else if (moveVelocityX < 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            data.playerDashBehaviour.SetDashDirection(Vector3.left);
        }
    }

    /// <summary>
    /// Funzione che gestisce il movimento
    /// </summary>
    /// <param name="_moveDirection"></param>
    void Move()
    {
        targetMoveVelocityX = moveDirection.x * currentMoveSpeed;
        moveVelocityX = Mathf.SmoothDamp(moveVelocityX, targetMoveVelocityX, ref velocityXSmoothing, moveSmoothingTime);
        moveVelocity = new Vector2(moveVelocityX, 0);
        data.playerRB.MovePosition(data.playerRB.position + moveVelocity * Time.fixedDeltaTime);
    }

    void CheckMoveVelocityX()
    {
        if (targetMoveVelocityX == 0)
            OnMovementStop.Invoke();
        else
            OnMovementStart.Invoke();
    }

    void CheckMoveVelocityY()
    {
        if (data.playerRB.velocity.y > 0)
            IsRising = true;
        else
        if (data.playerRB.velocity.y < 0)
            IsRising = false;
    }

    float interpolationValue;
    void EvaluateRunCurve(bool _isAccelerating)
    {
        if (_isAccelerating)
        {
            interpolationValue = runAccelerationCurve.Evaluate(accelTimer / accelerationTime);
            currentMoveSpeed = Mathf.Lerp(moveSpeed, runMoveSpeed, interpolationValue);
        }
        else
        {
            interpolationValue = runDecelerationCurve.Evaluate(decelTimer / decelerationTime);
            currentMoveSpeed = Mathf.Lerp(moveSpeed, runMoveSpeed, interpolationValue);
        }
    }

    #endregion

    #region API

    /// <summary>
    /// Funzione che setta la direzione di movimento
    /// </summary>
    /// <param name="_moveDirection"></param>
    public void SetMoveDirection(Vector3 _moveDirection)
    {
        moveDirection = _moveDirection;
    }

    public void HandleRunPress()
    {
        accelerating = true;
        decelerating = false;
        decelTimer = 0;
    }

    public void HandleRunRelease()
    {
        //Reset accel timer.
        accelerating = false;
        accelTimer = 0;

        //check ground collision
        if (data.playerCollisionBehaviour.Below)
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
    public void HandleGroundCollision(bool _value)
    {
        if (_value && runReleasedInAir && !groundCollisionDecelHandled)
        {
            decelerating = true;
            decelTimer = 0;
            groundCollisionDecelHandled = true;
        }
    }

    #endregion

}