using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFacingUpState : PlayerStateBase
{
    protected override void Enter()
    {
        myContext.myPlayer.SetIsFacingDown(false);
        myContext.myPlayer.SetIsFacingUp(true);
    }

    protected override void Tick()
    {
        if (myContext.myPlayer.GetIsFacingLeft())
        {
            myContext.myPlayer.SetIsFacingRight(Input.GetAxisRaw("Horizontal") == 1);

            if (myContext.myPlayer.GetIsFacingRight())
            {
                myContext.myPlayer.RotateEntity(Vector3.up, 180f);
            }
        }
        if (myContext.myPlayer.GetIsFacingRight())
        {
            myContext.myPlayer.SetIsFacingLeft(Input.GetAxisRaw("Horizontal") == -1);

            if (myContext.myPlayer.GetIsFacingLeft())
            {
                myContext.myPlayer.RotateEntity(Vector3.up, 180f);
            }
        }

        myContext.myPlayer.SetIsFacingDown(Input.GetAxisRaw("Vertical") == -1);
        myContext.myPlayer.SetIsFacingUp(!(Input.GetAxisRaw("Vertical") <= 0));
    }
}
