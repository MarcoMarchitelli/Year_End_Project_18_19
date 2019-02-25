using UnityEngine;

public class DamageDealerBehaviour : BaseBehaviour
{

    [SerializeField] protected bool dealsOnCollision = false;
    [SerializeField] protected bool dealsOnTrigger = false;
    [SerializeField] protected bool depleatesHealth = false;
    [SerializeField] protected int damage;

    #region Events
    [SerializeField] UnityFloatEvent OnDamageDealt;
    #endregion

    /// <summary>
    /// Funzione che infligge danno al receiver rilevato
    /// </summary>
    /// <param name="_receiver"></param>
    public void DealDamage(DamageReceiverBehaviour _receiver)
    {
        if (!depleatesHealth)
        {
            _receiver.SetHealth(-damage);
            OnDamageDealt.Invoke(damage);
        }
        else
        {
            _receiver.SetHealth(-_receiver.GetHealth());
        }
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