using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deirin.Utility;

[RequireComponent(typeof(DamageDealerBehaviour),typeof(BoxCollider))]
public class AttackBehaviour : BaseBehaviour
{
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
        damageDealerBehaviour = GetComponent<DamageDealerBehaviour>();
        damageCollider = GetComponent<BoxCollider>();

        repeatCDTimer = Instantiate(timerPrefab, transform);
        otherAttacksCDTimer = Instantiate(timerPrefab, transform);
        otherActionsCDTimer = Instantiate(timerPrefab, transform);
        durationTimer = Instantiate(timerPrefab, transform);

        repeatCDTimer.SetTime(repeatTime);
        otherAttacksCDTimer.SetTime(otherAttacksTime);
        otherActionsCDTimer.SetTime(otherActionsTime);
        durationTimer.SetTime(duration);

        durationTimer.OnTimerEnd.AddListener(Stop);

        OnAttackStart.AddListener(ActivateDamageCollider);

        OnAttackEnd.AddListener(otherAttacksCDTimer.StartTimer);
        OnAttackEnd.AddListener(otherActionsCDTimer.StartTimer);
        OnAttackEnd.AddListener(repeatCDTimer.StartTimer);
        OnAttackEnd.AddListener(DeactivateDamageCollider);
    }

    public void Play()
    {
        OnAttackStart.Invoke();
    }

    public void Stop()
    {
        OnAttackEnd.Invoke();
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