using UnityEngine;

public class PlayerMovementBehaviour : BaseBehaviour
{

    #region Serialized Fields

    /// <summary>
    /// Behaviour's movement speed;
    /// </summary>
    [SerializeField] float moveSpeed;
    /// <summary>
    /// Behaviour's movement smoothing time;
    /// </summary>
    [SerializeField] float moveSmoothingTime = .1f;
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

    Rigidbody rb;
    Vector3 moveVelocityVector;
    float moveVelocityX;
    float targetMoveVelocityX;
    float velocityXSmoothing;
    DashBehaviour dashBehaviour;

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

    protected override void CustomSetup()
    {
        rb = GetComponent<Rigidbody>();
        dashBehaviour = Entity.gameObject.GetComponentInChildren<DashBehaviour>();
        //WTFF
        dashBehaviour.SetDashDirection(Vector3.right);
    }

    public override void OnFixedUpdate()
    {
        if (IsSetupped)
        {
            Move();
            FaceMoveDirection();
            CheckMoveVelocity();
        }
    }

    public override void OnUpdate()
    {
        if (rb.velocity.y > 0)
            IsRising = true;
        else
        if (rb.velocity.y < 0)
            IsRising = false;
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
        moveVelocityX = Mathf.SmoothDamp(moveVelocityX, targetMoveVelocityX, ref velocityXSmoothing, moveSmoothingTime);
        moveVelocityVector = new Vector2(moveVelocityX, 0);
        rb.MovePosition(rb.position + moveVelocityVector * Time.fixedDeltaTime);
    }

    void CheckMoveVelocity()
    {
        if (targetMoveVelocityX == 0)
            OnMovementStop.Invoke();
        else
            OnMovementStart.Invoke();
    }

    #endregion

    #region API

    /// <summary>
    /// Funzione che setta la direzione di movimento
    /// </summary>
    /// <param name="_moveDirection"></param>
    public void SetMoveVelocity(Vector3 _moveDirection)
    {
        targetMoveVelocityX = _moveDirection.x * moveSpeed;
    }

    #endregion

}