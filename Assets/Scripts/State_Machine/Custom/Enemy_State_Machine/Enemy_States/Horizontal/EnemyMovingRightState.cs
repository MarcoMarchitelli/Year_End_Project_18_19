using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovingRightState : EnemyStateBase
{
    protected override void Tick()
    {
        if (myContext.myEnemy.myRayCon.Collisions.below)
        {
            myContext.myEnemy.ResetVerticalVelocity();
        }

        myContext.myEnemy.MoveRight();
    }
}
