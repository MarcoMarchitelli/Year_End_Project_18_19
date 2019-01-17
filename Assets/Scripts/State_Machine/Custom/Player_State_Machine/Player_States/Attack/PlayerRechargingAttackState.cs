using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRechargingAttackState : PlayerStateBase
{
    protected override void Tick()
    {
        myContext.myPlayer.RechargeAttack();
    }
}
