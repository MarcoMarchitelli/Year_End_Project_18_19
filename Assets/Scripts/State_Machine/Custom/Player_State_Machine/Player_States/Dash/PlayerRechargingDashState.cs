using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRechargingDashState : PlayerStateBase
{
    protected override void Tick()
    {
        myContext.myPlayer.RechargeDash();
    }
}
