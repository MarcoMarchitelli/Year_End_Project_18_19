using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovingLeftState : EnemyStateBase
{
    protected override void Tick()
    {
        if (myContext.myEnemy.myRayCon.Collisions.below)
        {
            myContext.myEnemy.ResetVerticalVelocity();
        }

        myContext.myEnemy.MoveLeft();
    }
}
