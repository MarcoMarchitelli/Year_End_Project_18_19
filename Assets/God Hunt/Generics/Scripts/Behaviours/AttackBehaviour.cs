using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deirin.Utility;

[RequireComponent(typeof(DamageDealerBehaviour),typeof(BoxCollider))]
public class AttackBehaviour : BaseBehaviour
{
    PlayerEntityData data;

    [Header("References")]
    [SerializeField] Timer timerPrefab;

    [Header("Stats")]
    [SerializeField] int damage;
    [SerializeField] float knockbackPower;
    [SerializeField] float duration;

    [Header("Times")]
    [SerializeField] float repeatTime;
    [SerializeField] float otherAttacksTime;
    [SerializeField] float otherActionsTime;

    [Header("Events")]
    [SerializeField] UnityVoidEvent OnAttackStart;
    [SerializeField] UnityVoidEvent OnAttackEnd;

    DamageDealerBehaviour damageDealerBehaviour;
    BoxCollider damageCollider;

    Timer repeatCDTimer;
    Timer otherAttacksCDTimer;
    Timer durationTimer;
    Timer otherActionsCDTimer;

    protected override void CustomSetup()
    {
        data = Entity.Data as PlayerEntityData;

        damageDealerBehaviour = GetComponent<DamageDealerBehaviour>();
        damageCollider = GetComponent<BoxCollider>();

        //timers intantiation
        repeatCDTimer = Instantiate(timerPrefab, transform);
        otherAttacksCDTimer = Instantiate(timerPrefab, transform);
        otherActionsCDTimer = Instantiate(timerPrefab, transform);
        durationTimer = Instantiate(timerPrefab, transform);

        //timers value assingment
        repeatCDTimer.SetTime(repeatTime);
        otherAttacksCDTimer.SetTime(otherAttacksTime);
        otherActionsCDTimer.SetTime(otherActionsTime);
        durationTimer.SetTime(duration);

        //internal events setup
        OnAttackStart.AddListener(ActivateDamageCollider);
        OnAttackStart.AddListener(data.playerInputBehaviour.AttackInputOff);
        OnAttackEnd.AddListener(DeactivateDamageCollider);

        //duration timer setup
        OnAttackStart.AddListener(durationTimer.StartTimer);
        durationTimer.OnTimerEnd.AddListener(Stop);

        //other attacks timer setup
        OnAttackEnd.AddListener(otherAttacksCDTimer.StartTimer);

        //other actions timer setup
        OnAttackEnd.AddListener(otherActionsCDTimer.StartTimer);

        //repeat timer setup
        repeatCDTimer.OnTimerEnd.AddListener(data.playerInputBehaviour.AttackInputOn);
        OnAttackEnd.AddListener(repeatCDTimer.StartTimer);
    }

    public void Play()
    {
        OnAttackStart.Invoke();
        print(name + " started");
    }

    public void Stop()
    {
        OnAttackEnd.Invoke();
        print(name + " ended");
    }

    public void ActivateDamageCollider()
    {
        damageCollider.enabled = true;
    }

    public void DeactivateDamageCollider()
    {
        damageCollider.enabled = false;
    }

}