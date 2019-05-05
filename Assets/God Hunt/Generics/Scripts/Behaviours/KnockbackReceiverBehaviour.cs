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

    public void Knockback(float _knockbackPower, float _speedMul, float _distMul, Vector2 _knockbackDirection)
    {
        if (!IsSetupped)
            return;

        float knockbackForce = Mathf.Max(1, _knockbackPower - mass);
        StartCoroutine(KnockbackDisplacement(_knockbackDirection, knockbackForce * _distMul, knockbackForce * _speedMul));
    }

    IEnumerator KnockbackDisplacement(Vector2 _knockbackDirection, float _knockbackDistValue, float _knockbackSpeedValue)
    {
        OnKnockbackReceived.Invoke();

        float spaceTraveled = (_knockbackDirection * _knockbackDistValue).magnitude;
        float travelTime = spaceTraveled / _knockbackSpeedValue;
        float timer = 0;

        print(name + " knockback start with " + travelTime + " traveltime");

        while (timer < travelTime)
        {
            timer += Time.deltaTime;

            KnockbackMove(_knockbackDirection, _knockbackSpeedValue);

            yield return null;
        }

        print(name + " knockback end");

        OnKnockbackEnd.Invoke();
    }

    public virtual void KnockbackMove(Vector2 _direction, float speed)
    {
        transform.Translate(_direction * speed * Time.deltaTime);
    }
}