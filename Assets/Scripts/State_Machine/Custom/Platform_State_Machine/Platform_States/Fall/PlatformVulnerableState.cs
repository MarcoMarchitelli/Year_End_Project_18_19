using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformVulnerableState : PlatformStateBase {

    protected override void Enter()
    {
        myContext.myPlatform.ResetCurrentHits();
    }

    protected override void Tick()
    {
        if (Input.GetKeyDown(KeyCode.C) && myContext.myPlatform.GetCanFall())
        {
            myContext.myPlatform.TakeDamage(1000);
        }
    }
}
