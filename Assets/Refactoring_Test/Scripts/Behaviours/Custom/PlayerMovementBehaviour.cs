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

    #endregion

    Rigidbody rb;
    Vector3 moveVelocityVector;
    float moveVelocityX;
    float targetMoveVelocityX;
    float velocityXSmoothing;

    protected override void CustomSetup()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnFixedUpdate()
    {
        if (IsSetupped)
        {
            Move();
            CheckMoveVelocity();
        }
    }

    #region Behaviour's Methods

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
        if (moveVelocityX == 0)
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