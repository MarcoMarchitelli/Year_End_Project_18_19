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
    [SerializeField] UnityVoidEvent OnChargedAttackInput;

    //[SerializeField] PlayerCameraTarget cameraTarget;

    [HideInInspector] public bool IsPressingJump;
    [HideInInspector] public bool FallingThrough;

    bool canDash = false;
    bool canAttack = false;
    bool hasChargeAttacked = false;
    bool countTime = false;
    float timer;

    protected override void CustomSetup()
    {
        data = Entity.Data as PlayerEntityData;
        SetDashInput(true);
        AttackInputOn();
    }

    public override void OnUpdate()
    {
        CountTime();
        ReadInputs();
    }

    #region Internals

    void ReadInputs()
    {
        if (!IsSetupped)
        {
            Debug.Log(name + " input not setupped!");
            return;
        }

        Vector2 directionalInput = new Vector2(Input.GetAxisRaw(InputManager.CurrentInputDevice + "Horizontal"), Input.GetAxisRaw(InputManager.CurrentInputDevice + "Vertical"));
        //cameraTarget.SetMoveDirection(directionalInput);
        if (Mathf.Abs(directionalInput.x) >= horizontalInputDeadzone)
        {
            directionalInput.x = Mathf.Sign(directionalInput.x);
        }
        else
        {
            directionalInput.x = 0;
        }
        data.playerGameplayBehaviour.SetDirectionalInput(directionalInput);
        data.playerAttacksBehaviour.SetDirection(directionalInput);

        if (Input.GetButtonDown(InputManager.CurrentInputDevice + "Jump"))
        {
            if (directionalInput.y <= -verticalInputDeadzone && data.playerCollisionsBehaviour.CollidingWithTraversable)
            {
                data.playerCollisionsBehaviour.SetFallingThrowPlatform();
            }
            else
            {
                data.playerGameplayBehaviour.OnJumpInputDown();
                IsPressingJump = true;
            }
        }
        if (Input.GetButtonUp(InputManager.CurrentInputDevice + "Jump"))
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
            if (timer >= chargeTime)
            {
                OnChargedAttackInput.Invoke();
                hasChargeAttacked = true;
                countTime = false;
                timer = 0;
            }
        }
    }

    void EvaluateAttackTime(Vector2 _directionalInput)
    {
        countTime = false;

        if (_directionalInput.y > 0)
            OnUpAttackInput.Invoke();
        else
        {
            if (timer < chargeTime)
            {
                OnSideAttackInput.Invoke();
            }
            else
            {
                OnChargedAttackInput.Invoke();
                hasChargeAttacked = true;
            }
        }
        timer = 0;
    }

    #endregion

    #region API

    public void SetDashInput(bool _value)
    {
        canDash = _value;
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