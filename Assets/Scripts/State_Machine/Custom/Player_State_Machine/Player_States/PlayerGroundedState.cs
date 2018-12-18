using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerStateBase
{
    protected override void Enter()
    {
        myContext.myPlayer.velocity.y = 0f;
    }

    protected override void Tick()
    {
        if (myContext.myPlayer.myRayCon.Collisions.below || myContext.myPlayer.myRayCon.Collisions.above)
        {
            myContext.myPlayer.velocity.y = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            myContext.myPlayer.Jump();
        }
    }
}