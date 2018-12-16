using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBase : StateMachineBehaviour {

    protected virtual void Enter()
    {
        
    }

    protected virtual void Tick()
    {

    }

    protected virtual void Exit()
    {

    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        Enter();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        Tick();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        Exit();
    }
}