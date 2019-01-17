using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStandingStillState : PlayerStateBase
{
    protected override void Tick()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            myContext.myPlayer.SetHorizontalVelocity(Mathf.Sign(Input.GetAxisRaw("Horizontal")));
        }
    }
}