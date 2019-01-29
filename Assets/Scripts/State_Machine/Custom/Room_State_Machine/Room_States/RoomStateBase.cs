using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoomStateBase : StateBase
{
    protected RoomContext myContext;

    public StateBase Setup(IStateMachineContext stateContext)
    {
        myContext = (stateContext as RoomContext);
        return this;
    }
}