using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovingRightState : EnemyStateBase
{
    protected override void Enter()
    {
        myContext.myEnemy.ResetFacingDirections();
        myContext.myEnemy.SetIsFacingRight(true);
        myContext.myEnemy.RotateEntity();
    }

    protected override void Tick()
    {
        if (myContext.myEnemy.myRayCon.Collisions.below)
        {
            myContext.myEnemy.ResetVerticalVelocity();
        }

        myContext.myEnemy.MoveRight();
    }
}
