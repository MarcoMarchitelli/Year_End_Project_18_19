using UnityEngine;

public class ChargerAnimatorProxy : MonoBehaviour
{
    [SerializeField] Animator animator;

    [Header("Events")]
    public UnityVoidEvent OnChargeAnimStart;

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

    public void ChargeStart()
    {
        animator.SetTrigger("Start Charge");
    }

    public void ChargeEnd()
    {
        animator.SetTrigger("End Charge");
    }

    public void Charge()
    {
        OnChargeAnimStart.Invoke();
    }
}