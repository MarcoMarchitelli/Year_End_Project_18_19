using UnityEngine;

namespace Refactoring
{
    [RequireComponent(typeof(PlayerGameplayBehaviour))]
    public class PlayerInputBehaviour : BaseBehaviour
    {
        PlayerEntityData data;

        [SerializeField] KeyCode JumpKey = KeyCode.Space;
        [SerializeField] KeyCode SprintKey = KeyCode.LeftShift;
        [SerializeField] KeyCode DashKey = KeyCode.K;
        [SerializeField] KeyCode AttackKey = KeyCode.J;

        [SerializeField] UnityVoidEvent OnAttackInput;

        protected override void CustomSetup()
        {
            data = Entity.Data as PlayerEntityData;
        }

        public override void OnUpdate()
        {
            Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            data.playerGameplayBehaviour.SetDirectionalInput(directionalInput);

            if (Input.GetButtonDown("Jump"))
            {
                data.playerGameplayBehaviour.OnJumpInputDown();
            }
            if (Input.GetButtonUp("Jump"))
            {
                data.playerGameplayBehaviour.OnJumpInputUp();
            }

            if (Input.GetButtonDown("Run"))
            {
                data.playerGameplayBehaviour.HandleSprintPress();
            }
            if (Input.GetButtonUp("Run"))
            {
                data.playerGameplayBehaviour.HandleSprintRelease();
            }

            if (Input.GetButtonDown("Dash"))
            {
                data.playerGameplayBehaviour.HandleDashPress();
            }

            if (Input.GetButtonDown("Attack"))
            {
                data.animatorProxy.Attack();
                OnAttackInput.Invoke();
            }
        }
    } 
}