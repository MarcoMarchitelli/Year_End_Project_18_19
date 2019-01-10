using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyStateBase : StateBase
{
    protected EnemyContext myContext;

    public StateBase Setup(IStateMachineContext stateContext)
    {
        myContext = (stateContext as EnemyContext);
        return this;
    }
}
