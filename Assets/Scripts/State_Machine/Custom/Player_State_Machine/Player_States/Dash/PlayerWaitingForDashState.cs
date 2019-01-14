using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWaitingForDashState : PlayerStateBase
{
    protected override void Tick()
    {
        if (Input.GetButtonDown("Dash"))
        {
            myContext.myPlayer.Dash();
        }
    }
}