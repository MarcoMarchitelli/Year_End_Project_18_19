using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovingRightState : PlayerStateBase
{
    protected override void Tick()
    {
        myContext.myPlayer.SetHorizontalVelocity(Input.GetAxisRaw("Horizontal"));

        if (Input.GetKey(KeyCode.LeftShift))
        {
            myContext.myPlayer.UpdateAccelerationTime();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            myContext.myPlayer.ResetAccelerationTime();
        }
    }

    protected override void Exit()
    {
        myContext.myPlayer.ResetAccelerationTime();
        myContext.myPlayer.ResetHorizontalVelocity();
    }
}