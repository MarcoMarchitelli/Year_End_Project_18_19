using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerStateBase
{
    protected override void Tick()
    {
        myContext.myPlayer.SetIsAttacking();
    }
}
