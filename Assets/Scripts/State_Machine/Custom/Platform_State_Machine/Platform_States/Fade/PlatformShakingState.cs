using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformShakingState : PlatformStateBase
{
    protected override void Tick()
    {
        myContext.myPlatform.ShakePlatform();
    }
}
