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
        UdateDirectionalBools();
        UpdateCollsionBools();
        UpdateStandingStillBool();
        UpdateDashBools();
        UpdateAttackBools();
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

    private void UdateDirectionalBools()
    {
        myAnim.SetBool("FacingLeft", myPlayer.GetIsFacingLeft());
        myAnim.SetBool("FacingRight", myPlayer.GetIsFacingRight());
        myAnim.SetBool("FacingUp", myPlayer.GetIsFacingUp());
        myAnim.SetBool("FacingDown", myPlayer.GetIsFacingDown());
    }

    private void UpdateStandingStillBool()
    {
        myAnim.SetBool("StandingStill", myPlayer.GetVelocity().x == 0 ? true : false);
    }

    private void UpdateDashBools()
    {
        myAnim.SetBool("Dashing", myPlayer.GetIsDashing());
        myAnim.SetBool("DashRecharging", myPlayer.GetIsDashRecharging());
    }

    private void UpdateAttackBools()
    {
        myAnim.SetBool("Attacking", myPlayer.GetIsAttacking());
        myAnim.SetBool("AttackRecharging", myPlayer.GetIsAttackRecharging());
    }

    protected override void FillStates()
    {
        foreach (StateBase state in myAnim.GetBehaviours<StateBase>())
        {
            states.Add((state as PlayerStateBase).Setup(context));
        }
    }
}
