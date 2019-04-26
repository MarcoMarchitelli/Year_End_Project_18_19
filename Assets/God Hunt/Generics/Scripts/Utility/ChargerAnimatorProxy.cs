using UnityEngine;

public class ChargerAnimatorProxy : MonoBehaviour
{
    [SerializeField] Animator animator;

    [Header("Events")]
    public UnityVoidEvent OnChargeAnimStart;
    public UnityVoidEvent OnAttackAnimEnd;

    bool _isWalking;
    public bool IsWalking
    {
        get { return _isWalking; }
        set
        {
            if(value != _isWalking)
            {
                _isWalking = value;
                animator.SetBool("Walking", _isWalking);
            }
        }
    }

    public void PlayerHit()
    {
        animator.SetTrigger("Player Hit");
    }

    public void Alert()
    {
        animator.SetTrigger("Alert");
    }

    public void Damaged()
    {
        animator.SetTrigger("Damaged");
    }

    public void Death()
    {
        animator.SetTrigger("Death");
    }

    public void Charge()
    {
        OnChargeAnimStart.Invoke();
    }

    public void AttackEnd()
    {
        OnAttackAnimEnd.Invoke();
    }

}