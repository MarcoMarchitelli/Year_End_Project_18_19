using UnityEngine;
using Deirin.Utility;

[RequireComponent(typeof(DamageDealerBehaviour), typeof(BoxCollider))]
public class AttackBehaviour : BaseBehaviour
{
    PlayerEntityData data;

    [Header("References")]
    [SerializeField] Timer timerPrefab;

    [Header("Stats")]
    [SerializeField] int damage;
    [SerializeField] float duration;
    [SerializeField] bool canTurn = false;

    [Header("Knockback", order = 2)]
    [SerializeField] float knockbackPower;
    [SerializeField] float speedMultiplier;
    [SerializeField] float distanceMultiplier;

    [Header("Times")]
    [SerializeField] float colliderActivationDelay;
    [SerializeField] float repeatTime;
    [SerializeField] float otherAttacksTime;
    [SerializeField] float otherActionsTime;

    [Header("Events")]
    [SerializeField] UnityVoidEvent OnAttackStart;
    [SerializeField] UnityVoidEvent OnAttackEnd;

    DamageDealerBehaviour damageDealerBehaviour;
    KnockbackDealerBehaviour knockbackDealerBehaviour;
    BoxCollider damageCollider;

    Timer repeatCDTimer;
    Timer otherAttacksCDTimer;
    Timer durationTimer;
    Timer otherActionsCDTimer;
    Timer colliderActivationDelayTimer;

    protected override void CustomSetup()
    {
        data = Entity.Data as PlayerEntityData;

        damageDealerBehaviour = GetComponent<DamageDealerBehaviour>();
        knockbackDealerBehaviour = GetComponent<KnockbackDealerBehaviour>();
        damageCollider = GetComponent<BoxCollider>();

        damageDealerBehaviour.SetDamage(damage);
        knockbackDealerBehaviour.SetKnockbackPower(knockbackPower);
        knockbackDealerBehaviour.speedMultiplier = speedMultiplier;
        knockbackDealerBehaviour.distanceMultiplier = distanceMultiplier;

        //timers intantiation
        repeatCDTimer = Instantiate(timerPrefab, transform);
        otherAttacksCDTimer = Instantiate(timerPrefab, transform);
        otherActionsCDTimer = Instantiate(timerPrefab, transform);
        durationTimer = Instantiate(timerPrefab, transform);
        colliderActivationDelayTimer = Instantiate(timerPrefab, transform);

        //timers value assingment
        repeatCDTimer.SetTime(repeatTime);
        otherAttacksCDTimer.SetTime(otherAttacksTime);
        otherActionsCDTimer.SetTime(otherActionsTime);
        durationTimer.SetTime(duration);
        colliderActivationDelayTimer.SetTime(colliderActivationDelay);

        //internal events setup
        OnAttackStart.AddListener(data.playerInputBehaviour.AttackInputOff);
        OnAttackEnd.AddListener(DeactivateDamageCollider);

        //delay timer setup
        OnAttackStart.AddListener(colliderActivationDelayTimer.StartTimer);
        colliderActivationDelayTimer.OnTimerEnd.AddListener(ActivateDamageCollider);

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
        data.playerInputBehaviour.ToggleDirectionalInput(canTurn);
        OnAttackStart.Invoke();
    }

    public void Stop()
    {
        data.playerInputBehaviour.ToggleDirectionalInput(true);
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