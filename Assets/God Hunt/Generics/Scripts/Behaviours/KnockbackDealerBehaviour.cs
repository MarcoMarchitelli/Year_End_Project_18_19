using UnityEngine;

public class KnockbackDealerBehaviour : BaseBehaviour
{
    [SerializeField] bool disableOnStart = false;
    [SerializeField] protected bool dealsOnCollision = false;
    [SerializeField] protected bool dealsOnTrigger = false;
    [SerializeField] protected float knockbackPower;
    public float speedMultiplier;
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
            Debug.LogWarning(name + "'s damage dealer behaviour is not setupped!");
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
            KnockbackReceiverBehaviour receiver = other.GetComponent<KnockbackReceiverBehaviour>();
            if (receiver)
                DealKnockback(receiver);
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (!IsSetupped)
            return;

        if (dealsOnCollision)
        {
            KnockbackReceiverBehaviour receiver = collision.collider.GetComponent<KnockbackReceiverBehaviour>();
            if (receiver)
                DealKnockback(receiver);
        }
    }
}