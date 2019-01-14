using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRisingState : PlayerStateBase
{
    protected override void Tick()
    {
        if (myContext.myPlayer.myRayCon.Collisions.above)
        {
            myContext.myPlayer.ResetVerticalVelocity();
        }

        if (Input.GetButtonDown("Jump"))
        {
            myContext.myPlayer.MultipleJump();
        }
    }
}
