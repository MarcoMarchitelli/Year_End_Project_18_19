using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerStateBase
{
    protected override void Tick()
    {
        if (myContext.myPlayer.myRayCon.Collisions.below)
        {
            myContext.GoBackwardCallBack();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            myContext.myPlayer.MultipleJump();
        }
    }
}
