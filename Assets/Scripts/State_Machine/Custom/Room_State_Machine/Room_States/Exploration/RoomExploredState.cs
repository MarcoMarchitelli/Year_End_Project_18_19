using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomExploredState : RoomStateBase
{
    protected override void Tick()
    {
        foreach (EnterRaycastRoom raycast in myContext.myRoom.myEnterRaycasts)
        {
            raycast.CheckEnterTrigger();
        }

        foreach (ExitRaycastRoom raycast in myContext.myRoom.myExitRaycasts)
        {
            raycast.CheckExitTrigger();
        }
    }
}