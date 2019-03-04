using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class StateMachineBase : MonoBehaviour {

    [HideInInspector]
    public Animator myAnim;
    protected List<StateBase> states;
    protected IStateMachineContext context;

    protected abstract void FillStates();
}