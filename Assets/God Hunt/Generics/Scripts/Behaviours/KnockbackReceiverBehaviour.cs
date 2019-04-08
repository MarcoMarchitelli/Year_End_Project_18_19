using UnityEngine;
using System.Collections;

public class KnockbackReceiverBehaviour : BaseBehaviour
{
    #region Events
    public UnityVoidEvent OnKnockbackReceived;
    public UnityVoidEvent OnKnockbackEnd;
    #endregion

    [SerializeField] float mass;

    public void SetMass(float _value)
    {
        mass = _value;
    }

    public void Knockback(float _knockbackPower, Vector2 _knockbackDirection)
    {
        if (!IsSetupped)
            return;

        float knockbackForce = Mathf.Max(0, _knockbackPower - mass);
        StartCoroutine(KnockbackDisplacement(_knockbackDirection, knockbackForce));
    }

    IEnumerator KnockbackDisplacement(Vector2 _knockbackDirection, float _knockbackForce) // i treat force as a speed
    {
        OnKnockbackReceived.Invoke();

        float spaceTraveled = ((Vector2)transform.position + _knockbackDirection * _knockbackForce).magnitude;
        float travelTime = spaceTraveled / _knockbackForce;
        float timer = 0;

        while (timer < travelTime)
        {
            timer += Time.deltaTime;

            KnockbackMove(_knockbackDirection, _knockbackForce);

            yield return null;
        }

        OnKnockbackEnd.Invoke();
    }

    public virtual void KnockbackMove(Vector2 _direction, float speed)
    {
        transform.Translate(_direction * speed * Time.deltaTime);
    }
}