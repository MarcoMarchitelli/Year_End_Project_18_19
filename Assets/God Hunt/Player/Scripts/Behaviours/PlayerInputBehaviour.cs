using UnityEngine;

[RequireComponent(typeof(PlayerGameplayBehaviour))]
public class PlayerInputBehaviour : BaseBehaviour
{
    PlayerEntityData data;

    [SerializeField] UnityVoidEvent OnSideAttackInput;
    [SerializeField] UnityVoidEvent OnUpAttackInput;
    [SerializeField] float chargeTime;
    [SerializeField] UnityVoidEvent OnChargedAttackInput;

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
        Vector2 directionalInput = new Vector2(Input.GetAxisRaw(GameManager.Instance.CurrentInputDevice + "Horizontal"), Input.GetAxisRaw(GameManager.Instance.CurrentInputDevice + "Vertical"));
        data.playerGameplayBehaviour.SetDirectionalInput(directionalInput);
        data.playerAttacksBehaviour.SetDirection(directionalInput);

        if (Input.GetButtonDown(GameManager.Instance.CurrentInputDevice + "Jump"))
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
        if (Input.GetButtonUp(GameManager.Instance.CurrentInputDevice + "Jump"))
        {
            data.playerGameplayBehaviour.OnJumpInputUp();
            IsPressingJump = false;
        }

        if (Input.GetButtonDown(GameManager.Instance.CurrentInputDevice + "Run"))
        {
            data.playerGameplayBehaviour.HandleSprintPress();
        }
        if (Input.GetButtonUp(GameManager.Instance.CurrentInputDevice + "Run"))
        {
            data.playerGameplayBehaviour.HandleSprintRelease();
        }

        if (canDash && Input.GetButtonDown(GameManager.Instance.CurrentInputDevice + "Dash"))
        {
            data.playerGameplayBehaviour.HandleDashPress();
        }

        if (canAttack && Input.GetButtonDown(GameManager.Instance.CurrentInputDevice + "Attack"))
        {
            hasChargeAttacked = false;
            countTime = true;
        }
        if (!hasChargeAttacked && canAttack && Input.GetButtonUp(GameManager.Instance.CurrentInputDevice + "Attack"))
        {
            EvaluateAttackTime(directionalInput);
        }
    }

    void CountTime()
    {
        if (countTime)
        {
            timer += Time.deltaTime;
            if(timer >= chargeTime)
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

    #endregion
}