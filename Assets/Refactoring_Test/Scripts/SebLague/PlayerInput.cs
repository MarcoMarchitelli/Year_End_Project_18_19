using UnityEngine;
using System.Collections;

namespace Refactoring
{
    [RequireComponent(typeof(Player))]
    public class PlayerInput : BaseBehaviour
    {

        PlayerEntityData data;

        protected override void CustomSetup()
        {
            data = Entity.Data as PlayerEntityData;
        }

        public override void OnUpdate()
        {
            Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            data.player.SetDirectionalInput(directionalInput);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                data.player.OnJumpInputDown();
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                data.player.OnJumpInputUp();
            }
        }
    } 
}