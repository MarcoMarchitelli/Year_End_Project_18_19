using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachineBase
{
    private Player_Entity myPlayer;

    private void Start()
    {
        myPlayer = GetComponent<Player_Entity>();
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
        myAnim.SetFloat("Velocity.y", myPlayer.velocity.y);
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
