using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachineBase
{
    private PlayerController myPlayer;

    private void Start()
    {
        myPlayer = GetComponent<PlayerController>();
        myAnim = GetComponent<Animator>();
        states = new List<StateBase>();
        context = new PlayerContext()
        {
            myPlayer = myPlayer
        };
        FillStates();
    }

    private void Update()
    {
        UpdateHorizontalVelocityFloat();
        UpdateVerticalVelocityFloat();
        UpdateCollsionBools();
        UpdateStandingStillBool();
        UpdateFacingDirectionBool();
        UpdateDashBools();
    }

    private void UpdateCollsionBools()
    {
        myAnim.SetBool("CollisionAbove", myPlayer.myRayCon.Collisions.above);
        myAnim.SetBool("CollisionBelow", myPlayer.myRayCon.Collisions.below);
        myAnim.SetBool("CollisionRight", myPlayer.myRayCon.Collisions.right);
        myAnim.SetBool("CollisionLeft", myPlayer.myRayCon.Collisions.left);
    }

    private void UpdateHorizontalVelocityFloat()
    {
        myAnim.SetFloat("Velocity.x", myPlayer.GetVelocity().x);
    }

    private void UpdateVerticalVelocityFloat()
    {
        myAnim.SetFloat("Velocity.y", myPlayer.GetVelocity().y);
    }

    private void UpdateStandingStillBool()
    {
        myAnim.SetBool("StandingStill", myPlayer.GetVelocity().x == 0 ? true : false);
    }

    private void UpdateFacingDirectionBool()
    {
        myAnim.SetBool("FacingRight", myPlayer.CheckFacingRightAndRotate() ? true : false);
        myAnim.SetBool("FacingLeft", myPlayer.CheckFacingRightAndRotate() ? false : true);
    }

    private void UpdateDashBools()
    {
        myAnim.SetBool("Dashing", myPlayer.GetIsDashing());
        myAnim.SetBool("DashRecharging", myPlayer.GetIsDashRecharging());
    }

    protected override void FillStates()
    {
        foreach (StateBase state in myAnim.GetBehaviours<StateBase>())
        {
            states.Add((state as PlayerStateBase).Setup(context));
        }
    }
}
