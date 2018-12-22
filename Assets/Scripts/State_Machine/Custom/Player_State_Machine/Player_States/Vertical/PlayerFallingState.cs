using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerStateBase
{
    protected override void Tick()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            myContext.myPlayer.MultipleJump();
        }
    }
}
