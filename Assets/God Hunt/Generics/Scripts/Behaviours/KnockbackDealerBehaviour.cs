using UnityEngine;

public class KnockbackDealerBehaviour : BaseBehaviour
{
    [SerializeField] bool disableOnStart = false;
    [SerializeField] LayerMask collisionMask;
    [SerializeField] protected bool dealsOnCollision = false;
    [SerializeField] protected bool dealsOnTrigger = false;
    [Min(0.01f)]
    [SerializeField] protected float knockbackPower;
    [Min(0.01f)]
    public float speedMultiplier;
    [Min(0.01f)]
    public float distanceMultiplier;

    #region Events
    [SerializeField] UnityFloatEvent OnKnockbackDealt;
    #endregion

    protected override void CustomSetup()
    {
        if (disableOnStart)
            Enable(false);
    }

    public void DealKnockback(KnockbackReceiverBehaviour _receiver)
    {
        if (!IsSetupped)
        {
            Debug.LogWarning(name + "'s knockback dealer behaviour is not setupped!");
            return;
        }

        Vector2 collisionDirection = (_receiver.transform.position - Entity.gameObject.transform.position).normalized;

        _receiver.Knockback(knockbackPower, knockbackPower * speedMultiplier, knockbackPower * distanceMultiplier, collisionDirection);
    }

    public void SetKnockbackPower(float _value)
    {
        knockbackPower = _value;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!IsSetupped)
            return;

        if (dealsOnTrigger)
        {
            if (collisionMask == (collisionMask | (1 << other.gameObject.layer)))
            {
                KnockbackReceiverBehaviour receiver = other.GetComponent<KnockbackReceiverBehaviour>();
                if (receiver)
                    DealKnockback(receiver);
            }
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (!IsSetupped)
            return;

        if (dealsOnCollision)
        {
            if (collisionMask == (collisionMask | (1 << collision.gameObject.layer)))
            {
                KnockbackReceiverBehaviour receiver = collision.collider.GetComponent<KnockbackReceiverBehaviour>();
                if (receiver)
                    DealKnockback(receiver);
            }
        }
    }
}