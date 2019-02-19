using UnityEngine;

public class DamageDealerBehaviour : BaseBehaviour
{
    #region Events
    [SerializeField] UnityFloatEvent OnDamageDealt;
    #endregion

    /// <summary>
    /// The amount of damage to inflict
    /// </summary>
    [SerializeField] protected int damage;
    [SerializeField] protected bool dealsOnCollision = false;
    [SerializeField] protected bool dealsOnTrigger = false;

    /// <summary>
    /// Funzione che infligge danno al receiver rilevato
    /// </summary>
    /// <param name="_receiver"></param>
    public void DealDamage(DamageReceiverBehaviour _receiver)
    {
        _receiver.SetHealth(-damage);
        OnDamageDealt.Invoke(damage);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (dealsOnTrigger)
        {
            DamageReceiverBehaviour receiver = other.GetComponent<DamageReceiverBehaviour>();
            if (receiver)
                DealDamage(receiver);
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (dealsOnCollision)
        {
            DamageReceiverBehaviour receiver = collision.collider.GetComponent<DamageReceiverBehaviour>();
            if (receiver)
                DealDamage(receiver);
        }
    }
}