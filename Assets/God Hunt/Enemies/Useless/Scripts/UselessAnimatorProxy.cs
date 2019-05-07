using UnityEngine;

[RequireComponent(typeof(Animator))]
public class UselessAnimatorProxy : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    bool _isWalking;
    public bool IsWalking
    {
        get { return _isWalking; }
        set
        {
            if (value != _isWalking)
            {
                _isWalking = value;
                animator.SetBool("IsWalking", _isWalking);
            }
        }
    }

    public void Death()
    {
        animator.SetTrigger("Death");
    }

}