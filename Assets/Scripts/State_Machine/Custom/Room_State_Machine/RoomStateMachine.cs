using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomStateMachine : StateMachineBase
{

    private RoomController myRoom;

    private void Start()
    {
        myRoom = GetComponent<RoomController>();
        myAnim = GetComponent<Animator>();
        states = new List<StateBase>();
        context = new RoomContext()
        {
            myRoom = myRoom
        };
        FillStates();
    }

    private void Update()
    {
        UpdateRoomState();
    }

    private void UpdateRoomState()
    {
        myAnim.SetBool("CurrentlyActive", myRoom.IsPlayerInThisRoom);
    }

    protected override void FillStates()
    {
        foreach (StateBase state in myAnim.GetBehaviours<StateBase>())
        {
            states.Add((state as RoomStateBase).Setup(context));
        }
    }
}
