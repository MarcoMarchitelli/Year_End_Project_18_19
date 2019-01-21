using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFallingState : PlatformStateBase {

    protected override void Tick()
    {
        if (myContext.myPlatform.GetCanFall())
        {
            myContext.myPlatform.myRayCon.CheckFallingCollision(myContext.myPlatform.myRayCon.fallingMask, myContext.myPlatform.FallingDamage);
        }

        myContext.myPlatform.FallPlatform();
    }

    protected override void Exit()
    {
        myContext.myPlatform.myRayCon.Collisions.ResetCollisionInfo();
    }
}
