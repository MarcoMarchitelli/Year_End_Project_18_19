using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackBehaviour : BaseBehaviour
{
    [Header("Attacks")]
    [SerializeField] Attack BaseAttack;
    [SerializeField] Attack ChargedAttack;

    float timer = 0;
    bool countTime = false;

    #region Overrides
    protected override void CustomSetup()
    {
        BaseAttack.Init();
        ChargedAttack.Init();
    }

    public override void OnUpdate()
    {
        if (countTime)
        {
            timer += Time.deltaTime;
            if (timer >= ChargedAttack.Charge)
                ChargedAttack.Play();
        }
    }
    #endregion

    #region Internals
    void EvaluateTime()
    {
        countTime = false;
        if (timer < ChargedAttack.Charge)
            BaseAttack.Play();
        else if(timer >= ChargedAttack.Charge)
            ChargedAttack.Play();
        timer = 0;
    }
    #endregion

    #region API
    public void HandleAttackPress()
    {
        countTime = true;
    }

    public void HandleAttackRelease()
    {
        EvaluateTime();
    }
    #endregion
}

[System.Serializable]
public class Attack
{
    public string Name;
    public GameObject ColliderObject;
    [Header("Stats")]
    public int Damage;
    public float Knockback;
    [Header("Times")]
    public float Duration;
    public float Charge;
    public float RepeatCD;
    public float OtherAttackCD;
    public float ActionCD;
    [HideInInspector] public DamageDealerBehaviour damageDealerBehaviour;

    public void Init()
    {
        if (!ColliderObject)
        {
            Debug.LogError(Name + " has no collider object connected!");
            return;
        }
        damageDealerBehaviour = ColliderObject.GetComponent<DamageDealerBehaviour>();
        damageDealerBehaviour.SetDamage(Damage);
        ColliderObject.SetActive(false);
    }

    public void Play()
    {
        ColliderObject.SetActive(true);
    }

    public void Stop()
    {
        ColliderObject.SetActive(false);
    }
}