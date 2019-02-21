using UnityEngine;

/// <summary>
/// Behaviour che si occupa di eseguire il dash
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class DashBehaviour : BaseBehaviour
{
    #region Events
    /// <summary>
    /// Evento lanciato all'inizio del dash
    /// </summary>
    [SerializeField] UnityVoidEvent OnDashStart;
    /// <summary>
    /// Evento lanciato alla fine del dash, passa il cooldown del dash
    /// </summary>
    [SerializeField] UnityFloatEvent OnDashEnd;
    #endregion

    /// <summary>
    /// The force applyed to the entity.
    /// </summary>
    [SerializeField] float dashForce = 5f;
    /// <summary>
    /// dash cooldown.
    /// </summary>
    [SerializeField] float dashCooldown = 3f;

    /// <summary>
    /// Riferimento al Rigidbody
    /// </summary>
    Rigidbody rBody;
    Vector3 dashDirection;
    float dashTime;
    bool isVelocityDecreasing;
    float velocityX;
    float oldVelocityX;
    bool _isDashing;
    bool IsDashing
    {
        get { return _isDashing; }
        set
        {
            if(_isDashing != value)
            {
                _isDashing = value;
                if (!_isDashing)
                {
                    OnDashEnd.Invoke(dashCooldown);
                }
                else
                {
                    OnDashStart.Invoke();
                }
            }
        }
    }

    protected override void CustomSetup()
    {
        rBody = GetComponent<Rigidbody>();
    }

    public override void OnUpdate()
    {
        if (dashDirection.x > 0)
        {
            velocityX = rBody.velocity.x;
            if (velocityX < oldVelocityX)
                isVelocityDecreasing = true;
            else
                isVelocityDecreasing = false;
        }
        else if(dashDirection.x < 0)
        {
            velocityX = rBody.velocity.x;
            if (velocityX > oldVelocityX)
                isVelocityDecreasing = true;
            else
                isVelocityDecreasing = false;
        }

        if (isVelocityDecreasing && rBody.velocity.x == 0)
        {
            IsDashing = false;
        }

        oldVelocityX = velocityX;
    }

    #region API

    /// <summary>
    /// Funzione che esegue il dash
    /// </summary>
    public void Dash()
    {
        if (IsSetupped && dashDirection != Vector3.zero)
        {
            StartDash();
        }
    }

    public void SetDashDirection(Vector3 _dashDirection)
    {
        dashDirection = _dashDirection;
    }

    #endregion

    void StartDash()
    {
        IsDashing = true;
        rBody.AddForce(dashDirection * dashForce, ForceMode.Impulse);
    }
}