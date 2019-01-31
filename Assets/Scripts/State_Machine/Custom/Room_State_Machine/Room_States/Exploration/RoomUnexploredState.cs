using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomUnexploredState : RoomStateBase
{
    protected override void Enter()
    {
        if (myContext.myRoom.myEnterRaycasts != null)
        {
            foreach (EnterRaycastRoom raycast in myContext.myRoom.myEnterRaycasts)
            {
                raycast.UpdateRaycastOrigins();
            }
        }

        if (myContext.myRoom.myExitRaycasts != null)
        {
            foreach (ExitRaycastRoom raycast in myContext.myRoom.myExitRaycasts)
            {
                raycast.UpdateRaycastOrigins();
            }
        }
    }
}