using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerStateBase
{
    protected override void Tick()
    {
        myContext.myPlayer.AddGravity();

        if (Input.GetButtonDown("Jump"))
        {
            myContext.myPlayer.MultipleJump();
        }
    }

    protected override void Exit()
    {
        myContext.myPlayer.ResetGravityTimer();
    }
}
