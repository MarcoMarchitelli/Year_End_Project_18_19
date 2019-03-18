using UnityEngine;

[RequireComponent(typeof(PlayerGameplayBehaviour))]
public class PlayerInputBehaviour : BaseBehaviour
{
    PlayerEntityData data;

    [SerializeField] UnityVoidEvent OnAttackInput;

    [HideInInspector] public bool IsPressingJump;
    [HideInInspector] public bool FallingThrough;

    bool canDash = false;

    protected override void CustomSetup()
    {
        data = Entity.Data as PlayerEntityData;
        SetDashInput(true);
    }

    public override void OnUpdate()
    {
        ReadInputs();
    }

    #region Internals

    void ReadInputs()
    {
        Vector2 directionalInput = new Vector2(Input.GetAxisRaw(GameManager.Instance.CurrentInputDevice + "Horizontal"), Input.GetAxisRaw(GameManager.Instance.CurrentInputDevice + "Vertical"));
        data.playerGameplayBehaviour.SetDirectionalInput(directionalInput);

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

        if (Input.GetButtonDown(GameManager.Instance.CurrentInputDevice + "Attack"))
        {
            data.animatorProxy.Attack();
            OnAttackInput.Invoke();
        }
    }

    #endregion

    #region API

    public void SetDashInput(bool _value)
    {
        canDash = _value;
    }

    #endregion
}