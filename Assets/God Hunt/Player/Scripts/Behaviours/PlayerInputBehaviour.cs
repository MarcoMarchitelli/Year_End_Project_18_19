using UnityEngine;

[RequireComponent(typeof(PlayerGameplayBehaviour))]
public class PlayerInputBehaviour : BaseBehaviour
{
    PlayerEntityData data;

    [Range(0, 1)]
    [SerializeField] float verticalInputDeadzone = .8f;
    [Range(0, 1)]
    [SerializeField] float horizontalInputDeadzone = .2f;

    [SerializeField] UnityVoidEvent OnSideAttackInput;
    [SerializeField] UnityVoidEvent OnUpAttackInput;
    [SerializeField] float chargeTime;
    [SerializeField] UnityVoidEvent OnChargedAttackStart;
    [SerializeField] UnityVoidEvent OnChargedAttackHit;

    [HideInInspector] public bool IsPressingJump;
    [HideInInspector] public bool FallingThrough;

    bool canDash = false;
    bool canJump = false;
    bool canAttack = false;
    bool canTurn = false;
    bool hasChargeAttacked = false;
    bool countTime = false;
    float timer;

    protected override void CustomSetup()
    {
        data = Entity.Data as PlayerEntityData;
        SetDashInput(true);
        SetJumpInput(true);
        ToggleDirectionalInput(true);
        AttackInputOn();
    }

    public override void OnUpdate()
    {
        CountTime();
        ReadInputs();
    }

    #region Internals

    Vector2 directionalInput;
    void ReadInputs()
    {
        if (!IsSetupped)
        {
            Debug.Log(name + " input not setupped!");
            return;
        }

        if (canTurn)
        {
            directionalInput = new Vector2(Input.GetAxisRaw(InputManager.CurrentInputDevice + "Horizontal"), Input.GetAxisRaw(InputManager.CurrentInputDevice + "Vertical"));
        }

        data.cameraTarget.SetMoveDirection(directionalInput, data.playerGameplayBehaviour.accelerating);

        if (Mathf.Abs(directionalInput.x) >= horizontalInputDeadzone)
        {
            directionalInput.x = Mathf.Sign(directionalInput.x);
        }
        else
        {
            directionalInput.x = 0;
        }
        if (Mathf.Abs(directionalInput.y) >= verticalInputDeadzone)
        {
            directionalInput.y = Mathf.Sign(directionalInput.y);
        }
        else
        {
            directionalInput.y = 0;
        }
        data.playerGameplayBehaviour.SetDirectionalInput(directionalInput);
        data.playerAttacksBehaviour.SetDirection(directionalInput);

        if (canJump && Input.GetButtonDown(InputManager.CurrentInputDevice + "Jump"))
        {
            if (directionalInput.y == -1 && data.playerCollisionsBehaviour.CollidingWithTraversable)
            {
                data.playerCollisionsBehaviour.SetFallingThrowPlatform();
            }
            else
            {
                data.playerGameplayBehaviour.OnJumpInputDown();
                IsPressingJump = true;
            }
        }
        if (canJump && Input.GetButtonUp(InputManager.CurrentInputDevice + "Jump"))
        {
            data.playerGameplayBehaviour.OnJumpInputUp();
            IsPressingJump = false;
        }

        if (Input.GetButtonDown(InputManager.CurrentInputDevice + "Run"))
        {
            data.playerGameplayBehaviour.HandleSprintPress();
        }
        if (Input.GetButtonUp(InputManager.CurrentInputDevice + "Run"))
        {
            data.playerGameplayBehaviour.HandleSprintRelease();
        }

        if (canDash && Input.GetButtonDown(InputManager.CurrentInputDevice + "Dash"))
        {
            data.playerGameplayBehaviour.HandleDashPress();
        }
        if (canDash && Input.GetButtonUp(InputManager.CurrentInputDevice + "Dash"))
        {
            data.playerGameplayBehaviour.HandleDashRelease();
        }

        if (canAttack && Input.GetButtonDown(InputManager.CurrentInputDevice + "Attack"))
        {
            hasChargeAttacked = false;
            countTime = true;
            OnChargedAttackStart.Invoke();
        }
        if (!hasChargeAttacked && canAttack && Input.GetButtonUp(InputManager.CurrentInputDevice + "Attack"))
        {
            EvaluateAttackTime(directionalInput);
        }
    }

    void CountTime()
    {
        if (countTime)
        {
            timer += Time.deltaTime;
        }
    }

    void EvaluateAttackTime(Vector2 _directionalInput)
    {
        countTime = false;

        if(timer >= chargeTime)
        {
            OnChargedAttackHit.Invoke();
            hasChargeAttacked = true;
        }
        else
        {
            if (_directionalInput.y > 0)
                OnUpAttackInput.Invoke();
            else
                OnSideAttackInput.Invoke();
        }

        timer = 0;
    }

    #endregion

    #region API

    public void ToggleDirectionalInput(bool _value)
    {
        canTurn = _value;
    }

    public void SetDashInput(bool _value)
    {
        canDash = _value;
    }

    public void SetJumpInput(bool _value)
    {
        canJump = _value;
    }

    public void AttackInputOff()
    {
        canAttack = false;
    }

    public void AttackInputOn()
    {
        canAttack = true;
    }

    public void ResetDirectionInput()
    {
        Vector2 directionalInput = Vector2.zero;
        data.playerGameplayBehaviour.SetDirectionalInput(directionalInput);
        data.playerAttacksBehaviour.SetDirection(directionalInput);
    }

    #endregion

}