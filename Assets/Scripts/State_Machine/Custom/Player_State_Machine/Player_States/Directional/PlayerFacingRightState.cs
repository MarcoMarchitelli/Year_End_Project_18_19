using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFacingRightState : PlayerStateBase
{
    protected override void Enter()
    {
        if (myContext.myPlayer.GetIsFacingLeft())
        {
            myContext.myPlayer.RotateEntity(Vector3.up, 180f);
        }
        myContext.myPlayer.ResetFacingDirections();
        myContext.myPlayer.SetIsFacingRight(true);
    }

    protected override void Tick()
    {
        myContext.myPlayer.SetIsFacingLeft(Input.GetAxisRaw("Horizontal") == -1);
        myContext.myPlayer.SetIsFacingUp(Input.GetAxisRaw("Vertical") == 1);
        myContext.myPlayer.SetIsFacingDown(Input.GetAxisRaw("Vertical") == -1);
    }
}
