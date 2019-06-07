using UnityEngine;

public class DamageDealerBehaviour : BaseBehaviour
{
    [SerializeField] bool disableOnStart = false;
    [SerializeField] LayerMask collisionMask;
    [SerializeField] protected bool dealsOnCollision = false;
    [SerializeField] protected bool dealsOnTrigger = false;
    [Tooltip("Sets the target's health to 0.")]
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
        if (!isEnabled)
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

    public void SetDamage(int _value)
    {
        damage = _value;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!isEnabled)
            return;

        if (dealsOnTrigger)
        {
            if (collisionMask == (collisionMask | (1 << other.gameObject.layer)))
            {
                DamageReceiverBehaviour receiver = other.GetComponent<DamageReceiverBehaviour>();
                if (receiver)
                    DealDamage(receiver);
            }
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (!isEnabled)
            return;

        if (dealsOnCollision)
        {
            if (collisionMask == (collisionMask | (1 << collision.gameObject.layer)))
            {
                DamageReceiverBehaviour receiver = collision.collider.GetComponent<DamageReceiverBehaviour>();
                if (receiver)
                    DealDamage(receiver);
            }
        }
    }
}