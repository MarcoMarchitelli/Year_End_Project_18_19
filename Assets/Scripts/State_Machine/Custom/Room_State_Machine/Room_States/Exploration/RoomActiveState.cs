using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomActiveState : RoomStateBase
{
    protected override void Tick()
    {
        if (myContext.myRoom.myEnterRaycasts != null)
        {
            foreach (EnterRaycastRoom raycast in myContext.myRoom.myEnterRaycasts)
            {
                raycast.CheckEnterTrigger();
            }
        }

        if (myContext.myRoom.myExitRaycasts != null)
        {
            foreach (ExitRaycastRoom raycast in myContext.myRoom.myExitRaycasts)
            {
                raycast.CheckExitTrigger();
            }
        }
    }
}
