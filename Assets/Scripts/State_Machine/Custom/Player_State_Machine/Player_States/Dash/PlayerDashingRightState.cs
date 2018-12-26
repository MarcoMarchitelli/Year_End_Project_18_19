using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashingRightState : PlayerStateBase
{
    protected override void Tick()
    {
        myContext.myPlayer.SetIsDashing();
    }
}
