using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTremblingState : PlatformStateBase {

    protected override void Tick()
    {
        myContext.myPlatform.TremblePlatform();
    }
}
