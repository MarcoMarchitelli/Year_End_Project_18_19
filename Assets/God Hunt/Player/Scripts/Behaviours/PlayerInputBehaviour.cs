using UnityEngine;

namespace Refactoring
{
    [RequireComponent(typeof(PlayerGameplayBehaviour))]
    public class PlayerInputBehaviour : BaseBehaviour
    {
        PlayerEntityData data;

        [SerializeField] UnityVoidEvent OnAttackInput;

        [HideInInspector] public bool IsPressingJump;
        [HideInInspector] public bool FallingThrough;

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