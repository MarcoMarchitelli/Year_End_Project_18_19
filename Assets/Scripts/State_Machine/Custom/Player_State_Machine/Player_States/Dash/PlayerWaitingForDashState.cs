using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWaitingForDashState : PlayerStateBase
{

    protected override void Tick()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            myContext.myPlayer.Dash();
        }
    }

}