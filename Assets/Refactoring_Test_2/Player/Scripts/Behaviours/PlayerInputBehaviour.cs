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

            if (Input.GetKeyDown(JumpKey))
            {
                data.playerGameplayBehaviour.OnJumpInputDown();
            }
            if (Input.GetKeyUp(JumpKey))
            {
                data.playerGameplayBehaviour.OnJumpInputUp();
            }

            if (Input.GetKeyDown(SprintKey))
            {
                data.playerGameplayBehaviour.HandleSprintPress();
            }
            if (Input.GetKeyUp(SprintKey))
            {
                data.playerGameplayBehaviour.HandleSprintRelease();
            }

            if (Input.GetKeyDown(DashKey))
            {
                data.playerGameplayBehaviour.HandleDashPress();
            }

            if (Input.GetKeyDown(AttackKey))
            {
                data.animatorProxy.Attack();
                OnAttackInput.Invoke();
            }
        }
    } 
}