using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFallingState : PlatformStateBase {

    protected override void Tick()
    {
        myContext.myPlatform.FallPlatform();

        if (myContext.myPlatform.GetCanFall())
        {
            myContext.myPlatform.myRayCon.CheckRaycastsBools(myContext.myPlatform.myRayCon.fallingMask);
        }
    }

    protected override void Exit()
    {
        myContext.myPlatform.myRayCon.Collisions.ResetCollisionInfo();
    }
}
