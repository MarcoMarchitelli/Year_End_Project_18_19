using UnityEngine;

public class PlayerInputBehaviour : BaseBehaviour
{

    #region Serialized Fields

    [SerializeField] KeyCode JumpKey;
    [SerializeField] KeyCode DashKey;
    [SerializeField] KeyCode AttackKey;

    [SerializeField] UnityVoidEvent OnJumpPressed;
    [SerializeField] UnityVoidEvent OnJumpReleased;
    [SerializeField] UnityVoidEvent OnDashPressed;
    [SerializeField] UnityVoidEvent OnAttackPressed;
    /// <summary>
    /// Evento lanciato al cambio di direzione dell'asse di input
    /// </summary>
    [SerializeField] UnityVector3Event OnDirectionUpdate;

    #endregion

    /// <summary>
    /// Direzione in cui viene mosso l'asse di input della direzione
    /// </summary>
    Vector3 _moveDirection;
    /// <summary>
    /// Propery che lancia un evento al cambio di direzione dell'input
    /// </summary>
    Vector3 MoveDirection
    {
        get { return _moveDirection; }
        set
        {
            if (_moveDirection != value)
            {
                _moveDirection = value;
                OnDirectionUpdate.Invoke(_moveDirection);
            }
        }
    }

    bool canMove;
    bool canDash;
    bool canJump;
    bool canAttack;

    protected override void CustomSetup()
    {
        canMove = true;
        canDash = true;
        canJump = true;
        canAttack = true;
    }

    public override void OnUpdate()
    {
        if (IsSetupped)
        {
            InputsCheck();
        }
    }

    void InputsCheck()
    {
        if (canJump)
        {
            if(Input.GetKeyDown(JumpKey))
                OnJumpPressed.Invoke();
            if (Input.GetKeyUp(JumpKey))
                OnJumpReleased.Invoke();
        }

        if (canDash && Input.GetKeyDown(DashKey))
        {
            OnDashPressed.Invoke();
        }

        if (canAttack && Input.GetKeyDown(AttackKey))
        {
            OnAttackPressed.Invoke();
        }

        if (canMove)
        {
            MoveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);
        }
    }

    #region API

    public void ToggleMovementInput(bool _value)
    {
        canMove = _value;
        MoveDirection = Vector3.zero;
    }

    public void ToggleJumpInput(bool _value)
    {
        canJump = _value;
    }

    public void ToggleDashInput(bool _value)
    {
        canDash = _value;
    }

    #endregion

}