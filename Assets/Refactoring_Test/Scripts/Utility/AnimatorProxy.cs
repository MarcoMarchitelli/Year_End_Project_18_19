using UnityEngine;

public class AnimatorProxy : MonoBehaviour
{
    [SerializeField] Animator animator;

    bool _isGrounded;
    public bool IsGrounded
    {
        get { return _isGrounded; }
        set
        {
            if (value != _isGrounded)
            {
                _isGrounded = value;
                animator.SetBool("isGrounded", _isGrounded);
            }
        }
    }

    bool _isDashing;
    public bool IsDashing
    {
        get { return _isDashing; }
        set
        {
            if (value != _isDashing)
            {
                _isDashing = value;
                animator.SetBool("isDashing", _isDashing);
            }
        }
    }

    bool _isRising;
    public bool IsRising
    {
        get { return _isRising; }
        set
        {
            if (value != _isRising)
            {
                _isRising = value;
                animator.SetBool("isRising", _isRising);
            }
        }
    }

    bool _isWalking;
    public bool IsWalking
    {
        get { return _isWalking; }
        set
        {
            if(value != _isWalking)
            {
                _isWalking = value;
                animator.SetBool("isWalking", _isWalking);
            }
        }
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
    }

}