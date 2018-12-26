using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateBase : StateBase {

    protected PlayerContext myContext;

    public StateBase Setup(IStateMachineContext stateContext)
    {
        myContext = (stateContext as PlayerContext);
        return this;
    }

}