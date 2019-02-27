using UnityEngine;
using System.Collections;

/// <summary>
/// Behaviour che si occupa di eseguire il dash
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerDashBehaviour : BaseBehaviour
{
    #region Serialized Fields

    [SerializeField] float dashDistance = 5f;
    [SerializeField] float dashDuration;

    /// <summary>
    /// Evento lanciato all'inizio del dash
    /// </summary>
    [SerializeField] UnityVoidEvent OnDashStart;
    /// <summary>
    /// Evento lanciato alla fine del dash, passa il cooldown del dash
    /// </summary>
    [SerializeField] UnityVoidEvent OnDashEnd;

    #endregion

    PlayerEntityData data;

    Vector3 dashDirection;
    float dashSpeed;
    const int airDashes = 1;
    int airDashesCount;

    bool _isDashing = false;
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
                    OnDashEnd.Invoke();
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
        data = Entity.Data as PlayerEntityData;

        OnDashStart.AddListener(DashStartHandler);
        OnDashEnd.AddListener(DashEndHandler);
        dashSpeed = dashDistance / dashDuration;
        airDashesCount = 0;
    }

    #region API

    /// <summary>
    /// Funzione che esegue il dash
    /// </summary>
    public void Dash()
    {
        if (IsSetupped && !IsDashing)
        {
            if (data.playerCollisionBehaviour.Below)
                StartDash();
            else if (airDashesCount < 1)
                StartDash(true);
        }
    }

    public void SetDashDirection(Vector3 _dashDirection)
    {
        dashDirection = _dashDirection;
    }

    public void ResetAirDashesCount()
    {
        airDashesCount = 0;
    }

    #endregion

    void StartDash(bool _isAirDash = false)
    {
        if (_isAirDash)
            airDashesCount++;
        StartCoroutine(DashRoutine());
    }

    void DashEndHandler()
    {
        data.playerRB.useGravity = true;
    }

    void DashStartHandler()
    {
        data.playerRB.useGravity = false;
    }

    IEnumerator DashRoutine()
    {
        IsDashing = true;

        float timer = 0;
        float tempDashSpeed = dashSpeed * dashDirection.x;

        while (timer <= dashDuration)
        {
            timer += Time.fixedDeltaTime;

            data.playerRB.velocity = new Vector2(tempDashSpeed, 0);

            yield return new WaitForFixedUpdate();
        }

        data.playerRB.velocity = Vector2.zero;

        IsDashing = false;

    }

}