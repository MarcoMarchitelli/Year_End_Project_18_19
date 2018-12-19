using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStandingStillState : PlayerStateBase
{

    protected override void Tick()
    {
        myContext.myPlayer.SetHorizontalVelocity(Input.GetAxisRaw("Horizontal"));
    }
}
