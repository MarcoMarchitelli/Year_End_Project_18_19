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
            myPlayer = myPlayer,
            GoBackwardCallBack = GoBackward
        };
        FillStates();
    }

    private void Update()
    {
        UpdateVerticalVelocityFloat();
    }

    private void UpdateVerticalVelocityFloat()
    {
        myAnim.SetFloat("Velocity.y", myPlayer.GetVelocity().y);
    }

    private void GoBackward()
    {
        myAnim.SetTrigger("GoBackward");
    }

    protected override void FillStates()
    {
        foreach (StateBase state in myAnim.GetBehaviours<StateBase>())
        {
            states.Add((state as PlayerStateBase).Setup(context));
        }
    }
}
