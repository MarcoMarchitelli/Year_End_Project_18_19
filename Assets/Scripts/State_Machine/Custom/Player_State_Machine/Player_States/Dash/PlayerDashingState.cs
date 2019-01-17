using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashingState : PlayerStateBase
{
    protected override void Tick()
    {
        myContext.myPlayer.SetIsDashing();
    }
}
