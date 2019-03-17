using UnityEngine;

public class DamageDealerBehaviour : BaseBehaviour
{
    [SerializeField] bool disableOnStart = false;
    [SerializeField] protected bool dealsOnCollision = false;
    [SerializeField] protected bool dealsOnTrigger = false;
    [SerializeField] protected bool depleatesHealth = false;
    [SerializeField] protected int damage;

    #region Events
    [SerializeField] UnityFloatEvent OnDamageDealt;
    #endregion

    protected override void CustomSetup()
    {
        if (disableOnStart)
            Enable(false);
    }

    /// <summary>
    /// Funzione che infligge danno al receiver rilevato
    /// </summary>
    /// <param name="_receiver"></param>
    public void DealDamage(DamageReceiverBehaviour _receiver)
    {
        if (!IsSetupped)
        {
            Debug.LogWarning(name + "'s damage dealer behaviour is not setupped!");
            return;
        }

        if (!depleatesHealth)
        {
            _receiver.SetHealth(-damage);
            OnDamageDealt.Invoke(damage);
        }
        else
        {
            _receiver.SetHealth(-_receiver.CurrentHealth);
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!IsSetupped)
            return;

        if (dealsOnTrigger)
        {
            DamageReceiverBehaviour receiver = other.GetComponent<DamageReceiverBehaviour>();
            if (receiver)
                DealDamage(receiver);
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (!IsSetupped)
            return;

        if (dealsOnCollision)
        {
            DamageReceiverBehaviour receiver = collision.collider.GetComponent<DamageReceiverBehaviour>();
            if (receiver)
                DealDamage(receiver);
        }
    }
}