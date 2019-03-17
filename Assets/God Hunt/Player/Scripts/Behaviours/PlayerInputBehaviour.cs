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
        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        data.playerGameplayBehaviour.SetDirectionalInput(directionalInput);

        if (Input.GetButtonDown("Jump"))
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
        if (Input.GetButtonUp("Jump"))
        {
            data.playerGameplayBehaviour.OnJumpInputUp();
            IsPressingJump = false;
        }

        if (Input.GetButtonDown("Run"))
        {
            data.playerGameplayBehaviour.HandleSprintPress();
        }
        if (Input.GetButtonUp("Run"))
        {
            data.playerGameplayBehaviour.HandleSprintRelease();
        }

        if (canDash && Input.GetButtonDown("Dash"))
        {
            data.playerGameplayBehaviour.HandleDashPress();
        }

        if (Input.GetButtonDown("Attack"))
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