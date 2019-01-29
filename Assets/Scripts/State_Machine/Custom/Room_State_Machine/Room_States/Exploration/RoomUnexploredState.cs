using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomUnexploredState : RoomStateBase
{
    protected override void Enter()
    {
        foreach (EnterRaycastRoom raycast in myContext.myRoom.myEnterRaycasts)
        {
            raycast.UpdateRaycastOrigins();
        }

        foreach (ExitRaycastRoom raycast in myContext.myRoom.myExitRaycasts)
        {
            raycast.UpdateRaycastOrigins();
        }
    }
}