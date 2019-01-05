using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformReturningState : PlatformStateBase
{
    protected override void Tick()
    {
        myContext.myPlatform.ReturnPlatform();
    }
}
