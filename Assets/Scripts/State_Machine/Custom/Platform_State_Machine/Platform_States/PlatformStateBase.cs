using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlatformStateBase : StateBase
{
    protected PlatformContext myContext;

    public StateBase Setup(IStateMachineContext stateContext)
    {
        myContext = (stateContext as PlatformContext);
        return this;
    }
}