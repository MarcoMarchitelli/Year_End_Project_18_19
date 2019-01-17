using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWaitingForAttackState : PlayerStateBase
{
    protected override void Tick()
    {
        if (Input.GetButtonDown("Attack"))
        {
            myContext.myPlayer.BasicAttack();
        }
    }
}
