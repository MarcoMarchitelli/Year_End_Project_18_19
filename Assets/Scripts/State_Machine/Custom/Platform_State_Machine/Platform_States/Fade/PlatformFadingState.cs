using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFadingState : PlatformStateBase
{
    protected override void Tick()
    {
        myContext.myPlatform.FadePlatform();
    }
}
