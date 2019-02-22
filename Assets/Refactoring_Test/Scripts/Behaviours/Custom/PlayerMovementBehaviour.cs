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
    float timeToReachRunSpeed;
    /// <summary>
    /// Describes how the move speed interpolates to run speed.
    /// </summary>
    [Tooltip("Describes how the move speed interpolates to run speed.")]
    [SerializeField]
    AnimationCurve runAccelerationCurve;
    /// <summary>
    /// Behaviour's movement smoothing time.
    /// </summary>
    [Tooltip("Behaviour's movement smoothing time.")]
    [SerializeField]
    float moveSmoothingTime = .1f;
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

    Rigidbody rb;
    DashBehaviour dashBehaviour;

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

    bool countRunTimer;
    float runTimer;

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
                }
                else
                {
                    OnEntityFalling.Invoke();
                }
            }
        }
    }

    #endregion

    protected override void CustomSetup()
    {
        rb = GetComponent<Rigidbody>();
        dashBehaviour = Entity.gameObject.GetComponentInChildren<DashBehaviour>();
        //WTFF
        dashBehaviour.SetDashDirection(Vector3.right);
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
        if (countRunTimer)
        {
            runTimer += Time.deltaTime;
            if (runTimer > timeToReachRunSpeed)
                runTimer = timeToReachRunSpeed;
            EvaluateRunCurve();
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
            dashBehaviour.SetDashDirection(Vector3.right);
        }
        else if (moveVelocityX < 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            dashBehaviour.SetDashDirection(Vector3.left);
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
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
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
        if (rb.velocity.y > 0)
            IsRising = true;
        else
        if (rb.velocity.y < 0)
            IsRising = false;
    }

    float interpolationValue;
    void EvaluateRunCurve()
    {
        interpolationValue = runAccelerationCurve.Evaluate(runTimer / timeToReachRunSpeed);
        currentMoveSpeed = Mathf.Lerp(moveSpeed, runMoveSpeed, interpolationValue);
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
        countRunTimer = true;
    }

    public void HandleRunRelease()
    {
        countRunTimer = false;
        runTimer = 0;
        currentMoveSpeed = moveSpeed;
    }

    #endregion

}