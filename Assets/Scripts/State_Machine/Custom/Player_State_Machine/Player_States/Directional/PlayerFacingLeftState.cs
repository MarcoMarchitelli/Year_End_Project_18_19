using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFacingLeftState : PlayerStateBase
{
    protected override void Enter()
    {
        if (myContext.myPlayer.GetIsFacingRight())
        {
            myContext.myPlayer.RotateEntity(Vector3.up, 180f);
        }
        myContext.myPlayer.ResetFacingDirections();
        myContext.myPlayer.SetIsFacingLeft(true);
    }

    protected override void Tick()
    {
        myContext.myPlayer.SetIsFacingRight(Input.GetAxisRaw("Horizontal") == 1);
        myContext.myPlayer.SetIsFacingUp(Input.GetAxisRaw("Vertical") == 1);
        myContext.myPlayer.SetIsFacingDown(Input.GetAxisRaw("Vertical") == -1);
    }
}
