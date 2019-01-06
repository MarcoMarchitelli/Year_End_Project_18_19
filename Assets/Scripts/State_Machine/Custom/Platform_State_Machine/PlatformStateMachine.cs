using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformStateMachine : StateMachineBase {

    private PlatformController myPlatform;

    private void Start()
    {
        myPlatform = GetComponent<PlatformController>();
        myAnim = GetComponent<Animator>();
        states = new List<StateBase>();
        context = new PlatformContext()
        {
            myPlatform = myPlatform
        };
        FillStates();
    }

    private void Update()
    {
        UpdateCollsionBools();
        UpdateFadingBool();
        UpdateReturningBool();
    }

    private void UpdateCollsionBools()
    {
        myAnim.SetBool("CollisionAbove", myPlatform.myRayCon.Collisions.above);
    }

    private void UpdateFadingBool()
    {
        myAnim.SetBool("Fading", myPlatform.isFading);
        myAnim.SetBool("CanFade", myPlatform.GetCanFade());
    }

    private void UpdateReturningBool()
    {
        myAnim.SetBool("Returning", myPlatform.isReturning);
    }

    protected override void FillStates()
    {
        foreach (StateBase state in myAnim.GetBehaviours<StateBase>())
        {
            states.Add((state as PlatformStateBase).Setup(context));
        }
    }
}
