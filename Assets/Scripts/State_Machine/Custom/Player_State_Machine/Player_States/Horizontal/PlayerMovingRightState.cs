using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovingRightState : PlayerStateBase
{
    protected override void Tick()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            myContext.myPlayer.SetHorizontalVelocity(Mathf.Sign(Input.GetAxisRaw("Horizontal")));
        }
        else
        {
            myContext.myPlayer.SetHorizontalVelocity(0f);
        }

        if (Input.GetButton("Run") && !myContext.myPlayer.myRayCon.Collisions.right)
        {
            myContext.myPlayer.UpdateAccelerationTime();
        }
        else if (Input.GetButtonUp("Run") || myContext.myPlayer.myRayCon.Collisions.right)
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