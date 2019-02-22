using UnityEngine;

public class PlayerRisingState : PlayerStateBase
{
    protected override void Tick()
    {
        if (myContext.myPlayer.myRayCon.Collisions.above)
        {
            myContext.myPlayer.ResetVerticalVelocity();
        }

        if (Input.GetButtonUp("Jump"))
        {
            myContext.myPlayer.JumpMin();
        }

        if (Input.GetButtonDown("Jump"))
        {
            myContext.myPlayer.MultipleJump();
        }
    }
}