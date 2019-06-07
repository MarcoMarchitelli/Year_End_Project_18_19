using UnityEngine;
using InputTest;
using GodHunt.Inputs;

[RequireComponent(typeof(PlayerGameplayBehaviour))]
public class PlayerInputBehaviour : BaseBehaviour
{
    PlayerEntityData data;

    [Header("Starting Abilites")]
    [SerializeField] bool doubleJump;
    [SerializeField] bool sprint;
    [SerializeField] bool dash;

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

    bool canRun = false;
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
        SetSprintInput(sprint);
        SetDashInput(dash);
        data.playerGameplayBehaviour.ToggleDoubleJump(doubleJump);
        SetJumpInput(true);
        ToggleDirectionalInput(true);
        AttackInputOn();

        ToggleInputEventsSubscription(true);
    }

    public override void OnUpdate()
    {
        CountTime();
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
        if (!isEnabled || data.animatorProxy.IsDead)
        {
            return;
        }
    }

    void ToggleInputEventsSubscription(bool _value)
    {
        if (_value)
        {
            InputManager.OnAttackPressed  += HandleAttackPress;
            InputManager.OnAttackReleased += HandleAttackRelease;
            InputManager.OnDashPressed    += HandleDashPress;
            InputManager.OnDashPressed    += HandleDashPress;
            InputManager.OnRunPressed     += HandleRunPress;
            InputManager.OnRunPressed     += HandleRunPress;
            InputManager.OnJumpPressed    += HandleJumpPress;
            InputManager.OnJumpPressed    += HandleJumpPress;

            InputManager.OnMovementInput  += HandleMovementInput;
        }
        else
        {
            InputManager.OnAttackPressed  -= HandleAttackPress;
            InputManager.OnAttackReleased -= HandleAttackRelease;
            InputManager.OnDashPressed    -= HandleDashPress;
            InputManager.OnDashPressed    -= HandleDashPress;
            InputManager.OnRunPressed     -= HandleRunPress;
            InputManager.OnRunPressed     -= HandleRunPress;
            InputManager.OnJumpPressed    -= HandleJumpPress;
            InputManager.OnJumpPressed    -= HandleJumpPress;
                                          
            InputManager.OnMovementInput  -= HandleMovementInput;
        }
    }

    void HandleMovementInput(Vector2 _dir)
    {
        if (canTurn)
        {
            directionalInput = _dir;
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
    }

    void HandleAttackPress()
    {
        if (canAttack)
        {
            isChargeing = true;
            hasChargeAttacked = false;
            countTime = true;
            OnChargedStart.Invoke();
        }
    }

    void HandleAttackRelease()
    {
        if (!hasChargeAttacked && canAttack && isChargeing)
        {
            isChargeing = false;
            EvaluateAttackTime(directionalInput);
        }
    }

    void HandleDashPress()
    {
        if (canDash)
        {
            isDashing = true;
            data.playerGameplayBehaviour.HandleDashPress();
        }
    }          
               
    void HandleDashRelease()
    {
        if (canDash && isDashing)
        {
            isDashing = false;
            data.playerGameplayBehaviour.HandleDashRelease();
        }
    }

    void HandleRunPress()
    {
        if (canRun)
        {
            isRunning = true;
            data.playerGameplayBehaviour.HandleSprintPress();
        }
    }          
               
    void HandleRunRelease()
    {
        if (isRunning)
        {
            data.playerGameplayBehaviour.HandleSprintRelease();
        }
    }

    void HandleJumpPress()
    {
        if (canJump)
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
    }          
               
    void HandleJumpRelease()
    {
        if (canJump && IsPressingJump)
        {
            data.playerGameplayBehaviour.OnJumpInputUp();
            IsPressingJump = false;
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

    public void SetSprintInput(bool _value)
    {
        canRun = _value;
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