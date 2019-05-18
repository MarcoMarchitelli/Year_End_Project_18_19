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
    [Tooltip("Gets called on charge input down")]
    [SerializeField] UnityVoidEvent OnChargedStart;
    [Tooltip("Gets called on charge input hold if charge time was enough to perform a charged attack")]
    [SerializeField] UnityVoidEvent OnChargeCompleted;
    [Tooltip("Gets called on charge input up if charge time was enough to perform a charged attack")]
    [SerializeField] UnityVoidEvent OnChargedAttackInput;

    [HideInInspector] public bool FallingThrough;

    bool canDash = false;
    bool canJump = false;
    bool canAttack = false;
    bool canTurn = false;
    bool hasChargeAttacked = false;
    bool countTime = false;
    float timer;

    [HideInInspector] public bool IsPressingJump;
    bool isChargeing;
    bool isDashing;
    bool isRunning;

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

    public override void Enable(bool _value)
    {
        if (!data.animatorProxy.IsDead)
        {
            base.Enable(_value);
        }
    }

    #region Internals

    Vector2 directionalInput;
    void ReadInputs()
    {
        if (!IsSetupped || data.animatorProxy.IsDead)
        {
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
        if (canJump && IsPressingJump && Input.GetButtonUp(InputManager.CurrentInputDevice + "Jump"))
        {
            data.playerGameplayBehaviour.OnJumpInputUp();
            IsPressingJump = false;
        }

        if (Input.GetButtonDown(InputManager.CurrentInputDevice + "Run"))
        {
            isRunning = true;
            data.playerGameplayBehaviour.HandleSprintPress();
        }
        if (isRunning && Input.GetButtonUp(InputManager.CurrentInputDevice + "Run"))
        {
            data.playerGameplayBehaviour.HandleSprintRelease();
        }

        if (canDash && Input.GetButtonDown(InputManager.CurrentInputDevice + "Dash"))
        {
            isDashing = true;
            data.playerGameplayBehaviour.HandleDashPress();
        }
        if (canDash && isDashing && Input.GetButtonUp(InputManager.CurrentInputDevice + "Dash"))
        {
            isDashing = false;
            data.playerGameplayBehaviour.HandleDashRelease();
        }

        if (canAttack && Input.GetButtonDown(InputManager.CurrentInputDevice + "Attack"))
        {
            isChargeing = true;
            hasChargeAttacked = false;
            countTime = true;
            OnChargedStart.Invoke();
        }
        if (!hasChargeAttacked && canAttack && isChargeing && Input.GetButtonUp(InputManager.CurrentInputDevice + "Attack"))
        {
            isChargeing = false;
            EvaluateAttackTime(directionalInput);
        }
    }

    bool hasCalledChargeEvent = false;
    void CountTime()
    {
        if (countTime)
        {
            timer += Time.deltaTime;
            if(hasCalledChargeEvent == false && timer >= chargeTime)
            {
                OnChargeCompleted.Invoke();
                hasCalledChargeEvent = true;
            }
        }
    }

    void EvaluateAttackTime(Vector2 _directionalInput)
    {
        countTime = false;

        if(timer >= chargeTime)
        {
            OnChargedAttackInput.Invoke();
            hasChargeAttacked = true;
            hasCalledChargeEvent = false;
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

    public void Interrupt()
    {
        ResetDirectionInput();

        isChargeing = false;
        isDashing = false;
        IsPressingJump = false;
        isRunning = false;

        hasCalledChargeEvent = false;
        countTime = false;
        timer = 0;

        data.animatorProxy.Interrupt();
    }

    #endregion

}