using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFacingDownState : PlayerStateBase
{
    protected override void Enter()
    {
        myContext.myPlayer.SetIsFacingUp(false);
        myContext.myPlayer.SetIsFacingDown(true);
    }

    protected override void Tick()
    {
        if (myContext.myPlayer.GetIsFacingLeft())
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                myContext.myPlayer.SetIsFacingRight(Mathf.Sign(Input.GetAxisRaw("Horizontal")) == 1);
            }

            if (myContext.myPlayer.GetIsFacingRight())
            {
                myContext.myPlayer.RotateEntity(Vector3.up, 180f);
            }
        }
        if (myContext.myPlayer.GetIsFacingRight())
        {
            myContext.myPlayer.SetIsFacingLeft(Mathf.Sign(Input.GetAxisRaw("Horizontal")) == -1);

            if (myContext.myPlayer.GetIsFacingLeft())
            {
                myContext.myPlayer.RotateEntity(Vector3.up, 180f);
            }
        }

        myContext.myPlayer.SetIsFacingDown(Mathf.Sign(Input.GetAxisRaw("Vertical")) == -1);
        myContext.myPlayer.SetIsFacingUp(!(Input.GetAxisRaw("Vertical") <= 0));
    }
}
