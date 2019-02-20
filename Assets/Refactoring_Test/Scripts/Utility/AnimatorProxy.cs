using UnityEngine;

public class AnimatorProxy : MonoBehaviour
{
    [SerializeField] Animator animator;

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

    bool _isJumping;
    public bool IsJumping
    {
        get { return _isJumping; }
        set
        {
            if (value != _isJumping)
            {
                _isJumping = value;
                animator.SetBool("isJumping", _isJumping);
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

}
